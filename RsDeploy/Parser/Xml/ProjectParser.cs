using RsDeploy.Execution;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RsDeploy.Parser.Xml
{
    public class ProjectParser
    {
        private IEnumerable<IParser> ChildParsers { get; set; }
        public IDictionary<string, string> DataSources { get; } = new Dictionary<string, string>();

        public ProjectParser(IEnumerable<IParser> childParsers)
        {
            ChildParsers = childParsers;
        }

        public ProjectParser()
        {
            var childParsers = new List<IParser>();
            childParsers.Add(new DataSourceParser(new DataSourceService()));
            childParsers.Add(new FolderParser(new FolderService()));
            childParsers.Add(new ReportParser(new ReportService()));
        }

        public void Execute(Stream stream)
        {
            var xmlDoc = new XmlDocument();
            using (StreamReader reader = new StreamReader(stream))
                xmlDoc.Load(reader);

            var root = xmlDoc.FirstChild.NextSibling;
            foreach (var childParser in ChildParsers)
            {
                childParser.Root = this;
                childParser.Parent = null;
                childParser.ParentPath = string.Empty;
                childParser.Execute(root);
            }
                
        }
    }
}
