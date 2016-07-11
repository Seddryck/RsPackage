using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SsrsDeploy
{
    class Options
    {
        [Option('u', "url", Required = true,
          HelpText = "Url of the webservice where to deploy the reports.")]
        public string Url { get; set; }

        // Omitting long name, default --verbose
        [Option('s', "source", Required = true,
          HelpText = "File containing the definition of artefacts to deploy.")]
        public string Source { get; set; }

        [Option('f', "folder", Required = true,
          HelpText = "Parent folder on SSRS where the reports and data sources are deployed.")]
        public string ParentFolder { get; set; }

        [Option('r', "root", Required = false,
          HelpText = "Root path of the folder containing all the artefacts")]
        public string Root { get; set; }
    }
}
