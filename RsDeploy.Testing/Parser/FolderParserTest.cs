using NUnit.Framework;
using Moq;
using RsDeploy.Execution;
using RsDeploy.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RsDeploy.Parser.Xml;
using System.IO;
using System.Reflection;
using System.Xml;

namespace RsDeploy.Testing.Parser
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
                        .GetManifestResourceStream("RsDeploy.Testing.Resources.BasicSample.xml"))
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
                        .GetManifestResourceStream("RsDeploy.Testing.Resources.MultiLevelSample.xml"))
            using (StreamReader reader = new StreamReader(stream))
                xmlDoc.Load(reader);

            var root = xmlDoc.FirstChild.NextSibling;
            parser.Execute(root);

            Mock.Get(service).Verify(s => s.Create(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(4));
        }

        [Test]
        public void ParseReportsMultipleFolderNode()
        {
            var stubFolderService = new Mock<FolderService>();
            stubFolderService.Setup(s => s.Create(It.IsAny<string>(), It.IsAny<string>())).Verifiable();
            var folderService = stubFolderService.Object;

            var stubReportService = new Mock<ReportService>();
            stubReportService.Setup(s => s.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Verifiable();
            var reportService = stubReportService.Object;

            var mockReportParser = new Mock<ReportParser>(reportService);
            mockReportParser.Setup(s => s.Execute(It.IsAny<XmlNode>())).Verifiable();
            var childParser = (IParser)mockReportParser.Object;

            var parser = new FolderParser(folderService, Enumerable.Repeat(childParser, 1));

            var xmlDoc = new XmlDocument();
            using (Stream stream = Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("RsDeploy.Testing.Resources.MultiLevelSample.xml"))
            using (StreamReader reader = new StreamReader(stream))
                xmlDoc.Load(reader);

            var root = xmlDoc.FirstChild.NextSibling;
            parser.Execute(root);

            Mock.Get(childParser).Verify(s => s.Execute(It.IsAny<XmlNode>()), Times.Exactly(4));
        }

    }
}
