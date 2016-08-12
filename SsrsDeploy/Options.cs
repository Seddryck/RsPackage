using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SsrsDeploy
{
    public class Options
    {
        [Option('u', "url", Required = true,
          HelpText = "Url of the webservice where to deploy the reports.")]
        public string Url { get; set; }

        // Omitting long name, default --verbose
        [Option('s', "solution", Required = true,
          HelpText = "File containing the definition of artefacts to deploy.")]
        public string SourceFile { get; set; }

        [Option('f', "folder", Required = true,
          HelpText = "Parent folder on SSRS where the reports and data sources are deployed.")]
        public string ParentFolder { get; set; }

        [Option('r', "resources", Required = false,
          HelpText = "Path of the local folder containing all the resources (reports, data sources, shared datasets). If missing the path of the solution will be assumed.")]
        public string ResourcePath { get; set; }

    }
}
