using SsrsDeploy.Parser.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SsrsDeploy.Parser
{
    public interface IParserPathable
    {
        string RootPath { get; set; }

    }
}
