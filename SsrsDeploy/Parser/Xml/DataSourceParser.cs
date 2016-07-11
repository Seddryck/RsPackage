using SsrsDeploy.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using System.IO;

namespace SsrsDeploy.Parser.Xml
{
    public class DataSourceParser : IParser, IParserPathable
    {
        private DataSourceService DataSourceService;
        private IEnumerable<IParser> ChildrenParsers;

        public ProjectParser Root { get; set; }
        public string RootPath { get; set; }
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
                path = path ?? $"{Root.NamingConvention.Apply(name)}.rds";
                if (!Path.IsPathRooted(path))
                    path = Path.Combine(RootPath ?? string.Empty, path);

                DataSourceService.Create(name, ParentPath, path);
                Root.DataSources.Add(name, $"{ParentPath}/{name}");
            }
        }
        
    }
}
