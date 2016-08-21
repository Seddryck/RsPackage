using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RsPackage.CommandLineArgs
{
    [Verb("build", HelpText = "Build a RsPac (zip file with manifest for deployment and all the ressources: reports, data sources, folders)")]

    public class BuildOptions : Options
    {
        
    }
}
