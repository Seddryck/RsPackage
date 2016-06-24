using RsDeploy.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;

namespace RsDeploy.Parser.Xml
{
    public class DataSourceParser : IParser
    {
        private DataSourceService DataSourceService;
        private IEnumerable<IParser> ChildrenParsers;

        public ProjectParser Root { get; set; }
        public IParser Parent { get; set; }
        public string ParentPath { get; set; }

        public DataSourceParser(DataSourceService DataSourceService)
        {
            this.DataSourceService = DataSourceService;
            ChildrenParsers = new List<IParser>();
        }

        public virtual void Execute(XmlNode node)
        {
            var DataSourceNodes = node.SelectNodes("./DataSource");
            foreach (XmlNode DataSourceNode in DataSourceNodes)
            {
                var name = DataSourceNode.Attributes["Name"].Value;
                var path = DataSourceNode.SelectSingleNode("./Path")?.InnerXml;
                path = path ?? $"{Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(name.ToLower()).Replace(" ", string.Empty)}.rds";

                DataSourceService.Create(name, ParentPath, path);
                Root.DataSources.Add(name, $"{ParentPath}/{name}");
            }
        }
        
    }
}
