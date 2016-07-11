using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RsDeploy.Parser.NamingConventions
{
    public class TitleToUnderscoreWord : INamingConvention
    {
        public string Apply(string value)
        {
            return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(value.ToLower().Replace(" ", "_"));
        }
    }
}
