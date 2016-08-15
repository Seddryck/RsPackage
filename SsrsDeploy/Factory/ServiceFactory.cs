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
            return new ReportService(rs);
        }

        public FolderService GetFolderService()
        {
            return new FolderService(rs);
        }

        public DataSourceService GetDataSourceService()
        {
            return new DataSourceService(rs);
        }

        
    }
}
