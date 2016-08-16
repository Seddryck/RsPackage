using RsPackage.Execution;
using RsPackage.Parser.NamingConventions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RsPackage.Parser.Xml
{
    public class ProjectParser
    {
        public string RootPath { get; internal set; }
        public string ParentFolder { get; internal set; }

        public INamingConvention NamingConvention { get; set; }

        private FolderService folderService;

        internal IList<IParser> ChildParsers { get; set; }
        public IDictionary<string, string> DataSources { get; } = new Dictionary<string, string>();

        public ProjectParser()
        {
            ChildParsers = new List<IParser>();
        }

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

    }
}
