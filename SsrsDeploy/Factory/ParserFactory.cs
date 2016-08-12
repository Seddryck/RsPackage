using SsrDeploy.Factory;
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

            var reportParser = new ReportParser(serviceFactory.GetReportService());
            var folderParser = new FolderParser(serviceFactory.GetFolderService());
            var dataSourceParser = new DataSourceParser(serviceFactory.GetDataSourceService());

            parser.ChildParsers.Add(reportParser);
            parser.ChildParsers.Add(folderParser);
            parser.ChildParsers.Add(dataSourceParser);

            return parser;
        }

        protected virtual string GetRootPath(Options options)
        {
            var rootPath = options.ResourcePath ?? Path.GetDirectoryName(options.SourceFile);
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
