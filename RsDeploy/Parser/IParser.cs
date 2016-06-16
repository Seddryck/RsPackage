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
        void Execute(XmlNode node, string parent);
    }
}
