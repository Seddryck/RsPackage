using RsPackage.Parser;
using RsPackage.Parser.NamingConventions;
using RsPackage.Parser.Xml;
using RsPackage.CommandLineArgs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RsPackage.Action;

namespace RsPackage.Factory
{
    class PublisherFactory
    {
        public Publisher GetPublisher(PublishOptions options)
        {
            var serviceBuilder = new ServiceBuilder();
            serviceBuilder.Setup(options);
            serviceBuilder.Build();

            var rootPath = GetRootPath(options);
            var parentFolder = GetParentFolder(options);
            var namingConvention = GetNamingConvention(options);


            var parser = new Publisher()
            {
                FolderService = serviceBuilder.GetFolderService(),
                StreamProvider = serviceBuilder.GetStreamProvider(),
                SourceFile = options.SourceFile.EndsWith(".rspac") ? "@[Project].manifest" : options.SourceFile,
                ParentFolder = parentFolder,
                RootPath = rootPath,
                NamingConvention = namingConvention
            };

            var policyParser = new PolicyParser(serviceBuilder.GetPolicyService());
            var dataSourceParser = new DataSourceParser(serviceBuilder.GetDataSourceService());
            var reportParser = new ReportParser(serviceBuilder.GetReportService(), new[] { policyParser });
            var folderParser = new FolderParser(serviceBuilder.GetFolderService(), new IParser[] { policyParser, dataSourceParser, reportParser });

            parser.ChildParsers.Add(dataSourceParser);
            parser.ChildParsers.Add(reportParser);
            parser.ChildParsers.Add(folderParser);

            return parser;
        }

        protected virtual string GetRootPath(PublishOptions options)
        {
            var rootPath = options.ResourcePath ?? Path.GetDirectoryName(options.SourceFile);
            rootPath = rootPath.EndsWith(Path.DirectorySeparatorChar.ToString()) ? rootPath : rootPath + Path.DirectorySeparatorChar;
            return rootPath;
        }

        protected virtual string GetParentFolder(PublishOptions options)
        {
            if (options.ParentFolder.Trim() == ".")
                return "/";
            else if (options.ParentFolder.Trim().StartsWith("/"))
                return options.ParentFolder.Trim();
            else
                return "/" + options.ParentFolder.Trim();
        }

        protected virtual INamingConvention GetNamingConvention(PublishOptions options)
        {
            return new TitleToUnderscoreWord();
        }
    }
}
