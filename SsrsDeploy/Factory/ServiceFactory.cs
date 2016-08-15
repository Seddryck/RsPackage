using SsrsDeploy;
using SsrsDeploy.ReportingService;
using SsrsDeploy.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SsrDeploy.Factory
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
            rs.Url = GetUrl(options);
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

        public static string GetUrl(Options options)
        {
            var baseUrl = options.Url;
            var builder = new UriBuilder(baseUrl);

            if (string.IsNullOrEmpty(builder.Scheme))
                builder.Scheme = Uri.UriSchemeHttp;

            if (builder.Scheme != Uri.UriSchemeHttp && builder.Scheme != Uri.UriSchemeHttps)
                throw new ArgumentException();

            if (!builder.Path.EndsWith("/ReportService2010.asmx"))
                builder.Path += (builder.Path.EndsWith("/") ? "" : "/") + "ReportService2010.asmx";

            if (builder.Path.Equals("/ReportService2010.asmx"))
                builder.Path = "/ReportServer" + builder.Path;

            if (builder.Uri.IsDefaultPort)
                return builder.Uri.GetComponents(UriComponents.AbsoluteUri & ~UriComponents.Port, UriFormat.UriEscaped);

            return builder.ToString();
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
