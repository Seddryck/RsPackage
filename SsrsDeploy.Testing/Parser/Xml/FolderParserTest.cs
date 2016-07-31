using NUnit.Framework;
using Moq;
using SsrsDeploy.Execution;
using SsrsDeploy.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SsrsDeploy.Parser.Xml;
using System.IO;
using System.Reflection;
using System.Xml;

namespace SsrsDeploy.Testing.Parser.Xml
{
    [TestFixture]
    public class FolderParserTest
    {
        [Test]
        public void ParseFolderNode()
        {
            var mock = new Mock<FolderService>();
            mock.Setup(s => s.Create(It.IsAny<string>(), It.IsAny<string>())).Verifiable();
            var service = mock.Object;

            var parser = new FolderParser(service);

            var xmlDoc = new XmlDocument();
            using (Stream stream = Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("SsrsDeploy.Testing.Resources.BasicSample.xml"))
            using (StreamReader reader = new StreamReader(stream))
                xmlDoc.Load(reader);

            var root = xmlDoc.FirstChild.NextSibling;
            parser.Execute(root);

            Mock.Get(service).Verify(s => s.Create(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void ParseMultipleFolderNode()
        {
            var mock = new Mock<FolderService>();
            mock.Setup(s => s.Create(It.IsAny<string>(), It.IsAny<string>())).Verifiable();
            var service = mock.Object;

            var parser = new FolderParser(service);

            var xmlDoc = new XmlDocument();
            using (Stream stream = Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("SsrsDeploy.Testing.Resources.MultiLevelSample.xml"))
            using (StreamReader reader = new StreamReader(stream))
                xmlDoc.Load(reader);

            var root = xmlDoc.FirstChild.NextSibling;
            parser.Execute(root);

            Mock.Get(service).Verify(s => s.Create(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(4));
        }

        [Test]
        public void ParseFoldersWhenMultipleFolderNodeAndCreateThem()
        {
            var mockFolderService = new Mock<FolderService>();
            mockFolderService.Setup(s => s.Create(It.IsAny<string>(), It.IsAny<string>())).Verifiable();
            var folderService = mockFolderService.Object;

            var stubReportService = new Mock<ReportService>();
            stubReportService.Setup(s => s.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Verifiable();
            var reportService = stubReportService.Object;

            var stubReportParser = new Mock<ReportParser>(reportService);
            stubReportParser.Setup(s => s.Execute(It.IsAny<XmlNode>())).Verifiable();
            var childParser = (IParser)stubReportParser.Object;

            var folderParser = new FolderParser(folderService, Enumerable.Repeat(childParser, 1));

            var xmlDoc = new XmlDocument();
            using (Stream stream = Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("SsrsDeploy.Testing.Resources.MultiLevelSample.xml"))
            using (StreamReader reader = new StreamReader(stream))
                xmlDoc.Load(reader);

            var root = xmlDoc.FirstChild.NextSibling;
            folderParser.Execute(root);

            Mock.Get(folderService).Verify(s => s.Create( "Analysis", null));
            Mock.Get(folderService).Verify(s => s.Create( "Low-level", null));
            Mock.Get(folderService).Verify(s => s.Create( "Realtime", "/Analysis"));
            Mock.Get(folderService).Verify(s => s.Create( "Confidential", "/Analysis/Realtime"));
        }

    }
}
