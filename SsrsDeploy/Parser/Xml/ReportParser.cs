using SsrsDeploy.Execution;
using SsrsDeploy.Parser.NamingConventions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;

namespace SsrsDeploy.Parser.Xml
{
    public class ReportParser : IParser, IParserPathable
    {
        private ReportService reportService;
        private IEnumerable<IParser> ChildrenParsers;

        public ProjectParser Root { get; set; }

        public INamingConvention NamingConvention
        {
            get
            {
                return (Root?.NamingConvention) ?? new TitleToCamelCase();
            }
        }

        public string RootPath { get; set; }
        public IParser Parent { get; set; }
        public string ParentPath { get; set; }

        public ReportParser(ReportService reportService)
        {
            this.reportService = reportService;
            ChildrenParsers = new List<IParser>();
        }

        public virtual void Execute(XmlNode node)
        {
            var reportNodes = node.SelectNodes("./Report");
            foreach (XmlNode reportNode in reportNodes)
            {
                var name = reportNode.Attributes["Name"].Value;

                var path = reportNode.SelectSingleNode("./Path")?.InnerXml;
                path = path ?? $"{NamingConvention.Apply(name)}.rdl";
                if (!Path.IsPathRooted(path))
                    path = Path.Combine(RootPath ?? string.Empty, path);

                var description = reportNode.SelectSingleNode("./Description")?.InnerXml;
                var hidden = bool.Parse(reportNode.Attributes["Hidden"]?.Value ?? bool.FalseString);

                reportService.Create(name, ParentPath, path, description, hidden, Root?.DataSources);
            }
        }
    }
}
