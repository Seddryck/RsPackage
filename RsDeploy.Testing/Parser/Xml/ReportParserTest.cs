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

namespace RsDeploy.Testing.Parser.Xml
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
                        .GetManifestResourceStream("RsDeploy.Testing.Resources.BasicSample.xml"))
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
                        .GetManifestResourceStream("RsDeploy.Testing.Resources.BasicSample.xml"))
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
                        .GetManifestResourceStream("RsDeploy.Testing.Resources.BasicSample.xml"))
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
                        .GetManifestResourceStream("RsDeploy.Testing.Resources.BasicSample.xml"))
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
                        .GetManifestResourceStream("RsDeploy.Testing.Resources.BasicSample.xml"))
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
                        .GetManifestResourceStream("RsDeploy.Testing.Resources.BasicSample.xml"))
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
                        .GetManifestResourceStream("RsDeploy.Testing.Resources.BasicSample.xml"))
            using (StreamReader reader = new StreamReader(stream))
                xmlDoc.Load(reader);

            var root = xmlDoc.FirstChild.NextSibling.SelectSingleNode("./Folder[@Name='Analysis']");
            parser.Execute(root);

            Mock.Get(service).Verify(s => s.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), true));
        }
    }
}
