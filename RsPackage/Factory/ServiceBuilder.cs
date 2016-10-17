using RsPackage;
using RsPackage.ReportingService;
using RsPackage.Execution;
using RsPackage.CommandLineArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RsPackage.Logging;
using RsPackage.StreamProvider;

namespace RsPackage.Factory
{
    public class ServiceBuilder
    {
        private PublishOptions options;
        private ReportingService2010 rs;
        private IStreamProvider streamProvider;
        private ILogger logger;
        private Dictionary<Type, BaseService> services;
        private bool isBuilt;

        public ServiceBuilder()
        {
            this.logger = new NullLogger();
            this.services = new Dictionary<Type, BaseService>();
            this.isBuilt = false;
        }
            
        public void Setup(PublishOptions options)
        {
            this.options = options;
            this.isBuilt = false;
        }

        public void Build()
        {
            this.rs = BuildReportingService(options);
            this.streamProvider = BuildStreamProvider(options);
            this.streamProvider.MessageSent += logger.WriteMessage;
            this.logger = BuildLogger(options);
            AddService(new ReportService(rs, streamProvider));
            AddService(new FolderService(rs));
            AddService(new DataSourceService(rs, streamProvider));
            AddService(new PolicyService(rs));
            AddService(new SharedDatasetService(rs, streamProvider));
            this.isBuilt = true;
        }

        private void AddService(BaseService service)
        {
            services.Add(service.GetType(), service);
            service.MessageSent += logger.WriteMessage;
        }

        protected virtual ILogger BuildLogger(Options options)
        {
            var loggerFactory = new LoggerFactory();
            return loggerFactory.Build(options);
        }

        protected virtual ReportingService2010 BuildReportingService(PublishOptions options)
        {
            var rs = new ReportingService2010();
            var urlBuilder = new UrlBuilder();
            urlBuilder.Setup(options);
            urlBuilder.Build();
            rs.Url = urlBuilder.GetUrl();
            rs.Credentials = System.Net.CredentialCache.DefaultCredentials;
            return rs;
        }

        protected virtual IStreamProvider BuildStreamProvider(PublishOptions options)
        {
            if (string.IsNullOrEmpty(options.SourceFile))
                throw new ArgumentException("The parameter 'source' is mandatory");

                if (options.SourceFile.EndsWith(".rspac"))
                return new ZipStreamProvider(options.SourceFile);
            else
                if (!string.IsNullOrEmpty(options.ResourcePath))
                    return new FileStreamProvider(options.ResourcePath);
                else
                    return new FileStreamProvider(Path.GetDirectoryName(options.SourceFile));
        }

        protected virtual BaseService GetService(Type type)
        {
            if (!isBuilt)
                throw new InvalidOperationException();

            if (services.ContainsKey(type))
                return services[type];
            else
                throw new ArgumentOutOfRangeException();
        }

        public ReportService GetReportService()
        {
            return (ReportService)GetService(typeof(ReportService));
        }

        public FolderService GetFolderService()
        {
            return (FolderService)GetService(typeof(FolderService));
        }

        public DataSourceService GetDataSourceService()
        {
            return (DataSourceService)GetService(typeof(DataSourceService));
        }

        public PolicyService GetPolicyService()
        {
            return (PolicyService)GetService(typeof(PolicyService));
        }

        public SharedDatasetService GetSharedDatasetService()
        {
            return (SharedDatasetService)GetService(typeof(SharedDatasetService));
        }

        public IStreamProvider GetStreamProvider()
        {
            return streamProvider;
        }
    }
}
