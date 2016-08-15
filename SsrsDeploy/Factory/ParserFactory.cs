using SsrDeploy.Factory;
using SsrsDeploy.Parser;
using SsrsDeploy.Parser.NamingConventions;
using SsrsDeploy.Parser.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SsrsDeploy.Factory
{
    class ParserFactory
    {
        public ProjectParser GetXmlParser(Options options)
        {
            var serviceFactory = new ServiceFactory(options);
            var rootPath = GetRootPath(options);
            var parentFolder = GetParentFolder(options);
            var namingConvention = GetNamingConvention(options);


            var parser = new ProjectParser()
            {
                ParentFolder = parentFolder,
                RootPath = rootPath,
                NamingConvention = namingConvention
            };

            var policyParser = new PolicyParser(serviceFactory.GetPolicyService());
            var dataSourceParser = new DataSourceParser(serviceFactory.GetDataSourceService());
            var reportParser = new ReportParser(serviceFactory.GetReportService(), new[] { policyParser });
            var folderParser = new FolderParser(serviceFactory.GetFolderService(), new IParser[] { policyParser, dataSourceParser, reportParser });
            
            parser.ChildParsers.Add(dataSourceParser);
            parser.ChildParsers.Add(reportParser);
            parser.ChildParsers.Add(folderParser);
            
            return parser;
        }

        protected virtual string GetRootPath(Options options)
        {
            var rootPath = options.Root ?? Path.GetDirectoryName(options.Source);
            rootPath = rootPath.EndsWith(Path.DirectorySeparatorChar.ToString()) ? rootPath : rootPath + Path.DirectorySeparatorChar;
            return rootPath;
        }

        protected virtual string GetParentFolder(Options options)
        {
            return options.ParentFolder;
        }

        protected virtual INamingConvention GetNamingConvention(Options options)
        {
            return new TitleToUnderscoreWord();
        }
    }
}
