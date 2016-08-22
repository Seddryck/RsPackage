using RsPackage.Action;
using RsPackage.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RsPackage.Parser.Xml
{
    public class FolderParser : IParser
    {
        private FolderService folderService;
        private List<IParser> ChildrenParsers;

        public MultipleFilesPublisher Root { get; set; }
        public IParser Parent { get; set; }
        public string ParentPath { get; set; }

        public FolderParser(FolderService folderService)
        {
            ChildrenParsers = new List<IParser>();
            this.folderService = folderService;
        }

        public FolderParser(FolderService folderService, IEnumerable<IParser> childParsers)
        {
            ChildrenParsers = new List<IParser>();
            ChildrenParsers.AddRange(childParsers);
            ChildrenParsers.ForEach(p => p.Parent = this);
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
                    parser.ParentPath = this.ParentPath == "/" ? $"/{name}" : $"{this.ParentPath}/{name}";
                    parser.Execute(folderNode);
                }

                var childFolderParser = (FolderParser)this.MemberwiseClone();
                childFolderParser.ParentPath = this.ParentPath=="/" ? $"/{name}" : $"{ParentPath}/{name}";
                childFolderParser.Execute(folderNode);
            }
        }
    }
}
