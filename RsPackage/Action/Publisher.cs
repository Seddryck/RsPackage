using RsPackage.Execution;
using RsPackage.Parser;
using RsPackage.Parser.NamingConventions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RsPackage.Action
{
    public class Publisher
    {
        public string RootPath { get; internal set; }
        public string ParentFolder { get; internal set; }

        public INamingConvention NamingConvention { get; set; }
        public IStreamProvider StreamProvider { get; set; }

        protected internal string SourceFile { get; set; }

        internal IList<IParser> ChildParsers { get; set; }
        public IDictionary<string, string> DataSources { get; } = new Dictionary<string, string>();
        public IDictionary<string, string> SharedDatasets { get; } = new Dictionary<string, string>();

        public Publisher()
        {
            ChildParsers = new List<IParser>();
        }

        public virtual void Execute()
        {
            using (var stream = StreamProvider.GetSolutionStream(SourceFile))
                this.Execute(stream);
        }

        protected internal void Execute(Stream stream)
        {
            var folderService = (FolderService)this.ChildParsers.Single(p => p.GetType() == typeof(FolderService));
            var path = "/";
            foreach(var folder in ParentFolder.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries))
            {
                folderService.Create(folder, path);
                path += path == "/" ? folder: "/" + folder;
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

        
    }
}
