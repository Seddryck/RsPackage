using SsrsDeploy.Execution;
using SsrsDeploy.Parser.NamingConventions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SsrsDeploy.Parser.Xml
{
    public class ProjectParser
    {
        public string RootPath { get; internal set; }
        public string ParentFolder { get; internal set; }

        public INamingConvention NamingConvention { get; internal set; }

        private FolderService folderService;

        internal IList<IParser> ChildParsers { get; set; }
        public IDictionary<string, string> DataSources { get; } = new Dictionary<string, string>();

        public ProjectParser(IEnumerable<IParser> childParsers)
        {
            ChildParsers = childParsers.ToList();
        }

        public ProjectParser(ReportingService.ReportingService2010 rs, string parentFolder, string rootPath, INamingConvention namingConvention)
        {
            var childParsers = new List<IParser>();
            var dataSourceService = new DataSourceService(rs);
            dataSourceService.MessageSent += WriteMessageToConsole;
            var dataSourceParser = new DataSourceParser(dataSourceService);
            dataSourceParser.Root = this;

            var reportService = new ReportService(rs);
            reportService.MessageSent += WriteMessageToConsole;
            var reportParser = new ReportParser(reportService);
            reportParser.Root = this;

            childParsers.Add(dataSourceParser);
            childParsers.Add(reportParser);
            var folderService = new FolderService(rs);
            folderService.MessageSent += WriteMessageToConsole;
            childParsers.Add(new FolderParser(folderService, new IParser[] { dataSourceParser, reportParser}));
            ChildParsers = childParsers;

            ParentFolder = parentFolder;
            RootPath = rootPath;
            NamingConvention = namingConvention;
        }

        public ProjectParser()
            : this(null, "/", string.Empty, new TitleToCamelCase())
        { }

        public void Execute(Stream stream)
        {
            var p = "/";
            foreach(var f in ParentFolder.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries))
            {
                folderService.Create(f, p);
                p += p == "/" ? f: "/" + f;
            }

            var xmlDoc = new XmlDocument();
            using (StreamReader reader = new StreamReader(stream))
                xmlDoc.Load(reader);

            var root = xmlDoc.FirstChild.NextSibling;
            foreach (var childParser in ChildParsers)
            {
                childParser.Root = this;
                childParser.Parent = null;
                childParser.ParentPath = ParentFolder;

                if (childParser is IParserPathable)
                    ((IParserPathable)childParser).RootPath = RootPath;

                childParser.Execute(root);
            }
                
        }

        public void WriteMessageToConsole(object sender, MessageEventArgs eventArgs)
        {
            switch (eventArgs.Level)
            {
                case MessageEventArgs.LevelOption.Information:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                case MessageEventArgs.LevelOption.Warning:
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                case MessageEventArgs.LevelOption.Error:
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                default:
                    break;
            }
            Console.WriteLine(eventArgs.Message);
        }
    }
}
