using RsDeploy.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;

namespace RsDeploy.Parser.Xml
{
    public class ReportParser : IParser
    {
        private ReportService reportService;
        private IEnumerable<IParser> ChildrenParsers;

        public ReportParser(ReportService reportService)
        {
            this.reportService = reportService;
            ChildrenParsers = new List<IParser>();
        }

        public void Execute(XmlNode node, string parent)
        {
            var reportNodes = node.SelectNodes("./Report");
            foreach (XmlNode reportNode in reportNodes)
            {
                var name = reportNode.Attributes["Name"].Value;
                var path = reportNode.SelectSingleNode("./Path")?.InnerXml;
                path = path ?? $"{Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(name.ToLower()).Replace(" ", string.Empty)}.rdl";

                var description = reportNode.SelectSingleNode("./Description")?.InnerXml;
                var hidden = bool.Parse(reportNode.Attributes["Hidden"]?.Value ?? bool.FalseString);

                reportService.Create(name, parent, path, description, hidden);
            }
        }
    }
}
