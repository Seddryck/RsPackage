using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RsPackage.CommandLineArgs
{
    public class Options
    {
        [Option('s', "solution", Required = true,
          HelpText = "File containing the definition of artefacts to deploy.")]
        public string SourceFile { get; set; }

        [Option('r', "resources", Required = false,
          HelpText = "Path of the local folder containing all the resources (reports, data sources, shared datasets). If missing the path of the solution will be assumed.")]
        public string ResourcePath { get; set; }

        [Option('l', "log", Required = false,
          HelpText = "Path of the local folder to redirect all the logs. If missing the logs will be displayed on the console.")]
        public string LogPath { get; set; }
    }
}
