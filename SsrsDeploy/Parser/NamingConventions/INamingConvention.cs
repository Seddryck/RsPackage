using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SsrsDeploy.Parser.NamingConventions
{
    public interface INamingConvention
    {
        string Apply(string value);
    }
}
