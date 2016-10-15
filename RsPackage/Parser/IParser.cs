using RsPackage.Action;
using RsPackage.Parser.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RsPackage.Parser
{
    public interface IParser
    {
        Publisher Root { get; set; }
        
        IParser Parent { get; set; }
        string ParentPath { get; set; }

        void Execute(XmlNode node);
    }
}
