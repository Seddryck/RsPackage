using RsDeploy.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RsDeploy.Parser.Xml
{
    public class FolderParser : IParser
    {
        private FolderService folderService;
        private List<IParser> ChildrenParsers;

        public ProjectParser Root { get; set; }
        public IParser Parent { get; set; }
        public string ParentPath { get; set; }

        public FolderParser(FolderService folderService)
        {
            ChildrenParsers = new List<IParser>();
            //ChildrenParsers.Add(new ReportParser());
            //ChildrenParsers.Add(new PolicyParser());
            this.folderService = folderService;
        }

        public FolderParser(FolderService folderService, IEnumerable<IParser> childParsers)
        {
            ChildrenParsers = new List<IParser>();
            ChildrenParsers.AddRange(childParsers);
            this.folderService = folderService;
        }


        public virtual void Execute(XmlNode node)
        {
            var folderNodes = node.SelectNodes("./Folder");
            foreach (XmlNode folderNode in folderNodes)
            {
                var name = folderNode.Attributes["Name"].Value;
                folderService.Create(name, ParentPath);
                foreach (var parser in ChildrenParsers)
                {
                    parser.ParentPath = $"{ParentPath}/{name}";
                    parser.Execute(node);
                }

                this.ParentPath = $"{ParentPath}/{name}";
                Execute(folderNode);
            }
        }
    }
}
