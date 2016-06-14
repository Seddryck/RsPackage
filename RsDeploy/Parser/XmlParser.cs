using RsDeploy.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RsDeploy.Parser
{
    class XmlParser
    {
        private XmlDocument doc;
        private FolderService folderService;
        private ReportService reportService;

        protected void GetFolders(XmlNode node, string parent)
        {
            var folders = node.SelectNodes("Folder");
            foreach (XmlNode folder in folders)
            {
                var name = folder.Attributes["Name"].Value;
                folderService.Create(name, parent);
                GetReports(node, parent);
                GetFolders(node, $"{parent}/{name}");
            }
        }
        protected void GetReports(XmlNode node, string parent)
        {
            var reports = node.SelectNodes("Report");
            foreach (XmlNode report in reports)
            {
                var name = report.Attributes["Name"].Value;
                var path = report.Attributes["Path"]?.Value;
                reportService.Create(name, parent, path);
            }
        }
    }
}
