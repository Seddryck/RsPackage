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
    public class ReportParserTest
    {
        [Test]
        public void ParseReportNode()
        {
            var mock = new Mock<ReportService>();
            mock.Setup(s => s.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Verifiable();
            var service = mock.Object;

            var parser = new ReportParser(service);

            var xmlDoc = new XmlDocument();
            using (Stream stream = Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("SsrsDeploy.Testing.Resources.BasicSample.xml"))
            using (StreamReader reader = new StreamReader(stream))
                xmlDoc.Load(reader);

            var root = xmlDoc.FirstChild.NextSibling.SelectSingleNode("./Folder[@Name='Analysis']");
            parser.Execute(root);

            Mock.Get(service).Verify(s => s.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        }

        [Test]
        public void ParseTwoReportNodes()
        {
            var mock = new Mock<ReportService>();
            mock.Setup(s => s.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Verifiable();
            var service = mock.Object;

            var parser = new ReportParser(service);

            var xmlDoc = new XmlDocument();
            using (Stream stream = Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("SsrsDeploy.Testing.Resources.BasicSample.xml"))
            using (StreamReader reader = new StreamReader(stream))
                xmlDoc.Load(reader);

            var root = xmlDoc.FirstChild.NextSibling;
            parser.Execute(root);

            Mock.Get(service).Verify(s => s.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Exactly(2));
        }

        [Test]
        public void ParseCorrectName()
        {
            var mock = new Mock<ReportService>();
            mock.Setup(s => s.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Verifiable();
            var service = mock.Object;

            var parser = new ReportParser(service);

            var xmlDoc = new XmlDocument();
            using (Stream stream = Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("SsrsDeploy.Testing.Resources.BasicSample.xml"))
            using (StreamReader reader = new StreamReader(stream))
                xmlDoc.Load(reader);

            var root = xmlDoc.FirstChild.NextSibling.SelectSingleNode("./Folder[@Name='Analysis']");
            parser.Execute(root);

            Mock.Get(service).Verify(s => s.Create("Company sales", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()));
        }

        [Test]
        public void ParseCorrectParent()
        {
            var mock = new Mock<ReportService>();
            mock.Setup(s => s.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Verifiable();
            var service = mock.Object;

            var parser = new ReportParser(service);
            parser.ParentPath = "/Root/First-Child";

            var xmlDoc = new XmlDocument();
            using (Stream stream = Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("SsrsDeploy.Testing.Resources.BasicSample.xml"))
            using (StreamReader reader = new StreamReader(stream))
                xmlDoc.Load(reader);

            var root = xmlDoc.FirstChild.NextSibling.SelectSingleNode("./Folder[@Name='Analysis']");
            parser.Execute(root);

            Mock.Get(service).Verify(s => s.Create(It.IsAny<string>(),"/Root/First-Child", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()));
        }

        [Test]
        public void ParseCorrectPathWhenUnspecified()
        {
            var mock = new Mock<ReportService>();
            mock.Setup(s => s.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Verifiable();
            var service = mock.Object;

            var parser = new ReportParser(service);

            var xmlDoc = new XmlDocument();
            using (Stream stream = Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("SsrsDeploy.Testing.Resources.BasicSample.xml"))
            using (StreamReader reader = new StreamReader(stream))
                xmlDoc.Load(reader);

            var root = xmlDoc.FirstChild.NextSibling;
            parser.Execute(root);

            Mock.Get(service).Verify(s => s.Create(It.IsAny<string>(), It.IsAny<string>(), "DepartmentSales.rdl", It.IsAny<string>(), It.IsAny<bool>()));
        }

        [Test]
        public void ParseCorrectHiddenWhenUnspecified()
        {
            var mock = new Mock<ReportService>();
            mock.Setup(s => s.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Verifiable();
            var service = mock.Object;

            var parser = new ReportParser(service);

            var xmlDoc = new XmlDocument();
            using (Stream stream = Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("SsrsDeploy.Testing.Resources.BasicSample.xml"))
            using (StreamReader reader = new StreamReader(stream))
                xmlDoc.Load(reader);

            var root = xmlDoc.FirstChild.NextSibling;
            parser.Execute(root);

            Mock.Get(service).Verify(s => s.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), false));
        }

        [Test]
        public void ParseCorrectHiddenWhenSpecified()
        {
            var mock = new Mock<ReportService>();
            mock.Setup(s => s.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Verifiable();
            var service = mock.Object;

            var parser = new ReportParser(service);

            var xmlDoc = new XmlDocument();
            using (Stream stream = Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("SsrsDeploy.Testing.Resources.BasicSample.xml"))
            using (StreamReader reader = new StreamReader(stream))
                xmlDoc.Load(reader);

            var root = xmlDoc.FirstChild.NextSibling.SelectSingleNode("./Folder[@Name='Analysis']");
            parser.Execute(root);

            Mock.Get(service).Verify(s => s.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), true));
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

            var folderParser = new FolderParser(folderService, Enumerable.Repeat(childParser, 1));

            var xmlDoc = new XmlDocument();
            using (Stream stream = Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("SsrsDeploy.Testing.Resources.MultiLevelSample.xml"))
            using (StreamReader reader = new StreamReader(stream))
                xmlDoc.Load(reader);

            var root = xmlDoc.FirstChild.NextSibling;
            folderParser.Execute(root);

            Mock.Get(childParser).Verify(s => s.Execute(It.IsAny<XmlNode>()), Times.Exactly(4));
        }
    }
}
