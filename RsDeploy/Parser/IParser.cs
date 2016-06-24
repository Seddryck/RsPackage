using RsDeploy.Parser.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RsDeploy.Parser
{
    public interface IParser
    {
        ProjectParser Root { get; set; }
        IParser Parent { get; set; }
        string ParentPath { get; set; }

        void Execute(XmlNode node);
    }
}
