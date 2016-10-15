using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RsPackage.CommandLineArgs
{
    [Verb("publish", HelpText = "Publish contents (reports, data sources, folders) to an instance of ReportingService.")]

    public class PublishOptions : Options
    {
        [Option('u', "url", Required = true,
          HelpText = "Url of the webservice where to deploy the reports.")]
        public string Url { get; set; }

        [Option('f', "folder", Required = true,
          HelpText = "Parent folder on SSRS where the reports and data sources are deployed.")]
        public string ParentFolder { get; set; }

    }
}
