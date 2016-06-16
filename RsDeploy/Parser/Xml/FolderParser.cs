using RsDeploy.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RsDeploy.Parser.Xml
{
    class FolderParser : IParser
    {
        private FolderService folderService;
        private IList<IParser> ChildrenParsers;

        public FolderParser()
        {
            ChildrenParsers = new List<IParser>();
            //ChildrenParsers.Add(new ReportParser());
            //ChildrenParsers.Add(new PolicyParser());
        }

        public void Execute(XmlNode node, string parent)
        {
            var folderNodes = node.SelectNodes("Folder");
            foreach (XmlNode folderNode in folderNodes)
            {
                var name = folderNode.Attributes["Name"].Value;
                folderService.Create(name, parent);
                foreach (var parser in ChildrenParsers)
                    parser.Execute(node, parent);
                Execute(node, $"{parent}/{name}");
            }
        }
    }
}
