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
        private RoleService roleService;
        private PolicyService policyService;

        protected void GetRoles(XmlNode node)
        {
            var roleNodes = node.SelectNodes("Role");
            foreach (XmlNode roleNode in roleNodes)
            {
                var name = roleNode.Attributes["Name"].Value;
                var description = roleNode.SelectSingleNode("Description")?.Value;
                var tasks = new List<string>();
                var taskNodes = roleNode.SelectNodes("Task");
                foreach (XmlNode taskNode in taskNodes)
                    tasks.Add(taskNode.Value);

                roleService.Create(name, description, tasks.ToArray());
            }
        }

        protected void GetPolicy(XmlNode node, string parent)
        {
            var membershipNodes = node.SelectNodes("Membership");
            if (membershipNodes.Count == 0)
                return;

            var securities = new List<Tuple<string, string[]>>();
            foreach (XmlNode membershipNode in membershipNodes)
            {
                var role = membershipNode.Attributes["Role"].Value;
                var members = new List<string>();
                foreach (XmlNode memberNode in membershipNode.SelectNodes("Task"))
                    members.Add(memberNode.Value);
                var security = new Tuple<string, string[]>(role, members.ToArray());
                securities.Add(security);
            }

            policyService.Create(parent, securities);
        }

        protected void GetFolders(XmlNode node, string parent)
        {
            var folderNodes = node.SelectNodes("Folder");
            foreach (XmlNode folderNode in folderNodes)
            {
                var name = folderNode.Attributes["Name"].Value;
                folderService.Create(name, parent);
                GetReports(node, parent);
                GetFolders(node, $"{parent}/{name}");
            }
        }

        protected void GetReports(XmlNode node, string parent)
        {
            var reportNodes = node.SelectNodes("Report");
            foreach (XmlNode reportNode in reportNodes)
            {
                var name = reportNode.Attributes["Name"].Value;
                var path = reportNode.Attributes["Path"]?.Value;
                reportService.Create(name, parent, path);
            }
        }
    }
}
