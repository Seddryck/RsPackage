using SsrsDeploy;
using SsrsDeploy.ReportingService;
using SsrsDeploy.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SsrsDeploy.Factory
{
    class ServiceFactory
    {
        private readonly ReportingService2010 rs;

        public ServiceFactory(Options options)
        {
            this.rs = GetReportingService(options);
        }

        protected virtual ReportingService2010 GetReportingService(Options options)
        {
            var rs = new ReportingService2010();
            var urlBuilder = new UrlBuilder();
            urlBuilder.Setup(options);
            urlBuilder.Build();
            rs.Url = urlBuilder.GetUrl();
            rs.Credentials = System.Net.CredentialCache.DefaultCredentials;
            return rs;
        }

        public ReportService GetReportService()
        {
            var service = new ReportService(rs);
            service.MessageSent += WriteMessageToConsole;
            return service;
        }

        public FolderService GetFolderService()
        {
            var service = new FolderService(rs);
            service.MessageSent += WriteMessageToConsole;
            return service;
        }

        public DataSourceService GetDataSourceService()
        {
            var service = new DataSourceService(rs);
            service.MessageSent += WriteMessageToConsole;
            return service;
        }

        public PolicyService GetPolicyService()
        {
            var service = new PolicyService(rs);
            service.MessageSent += WriteMessageToConsole;
            return service;
        }


        public void WriteMessageToConsole(object sender, MessageEventArgs eventArgs)
        {
            switch (eventArgs.Level)
            {
                case MessageEventArgs.LevelOption.Information:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                case MessageEventArgs.LevelOption.Warning:
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                case MessageEventArgs.LevelOption.Error:
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                default:
                    break;
            }
            Console.WriteLine(eventArgs.Message);
        }
    }
}
