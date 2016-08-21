using NUnit.Framework;
using Moq;
using RsPackage.Execution;
using RsPackage.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RsPackage.Parser.Xml;
using System.IO;
using System.Reflection;
using System.Xml;
using RsPackage.Parser.NamingConventions;

namespace RsPackage.Testing.Parser.Xml
{
    [TestFixture]
    public class SharedDatasetParserTest
    {
        [Test]
        public void SharedDatasetNode()
        {
            var mock = new Mock<SharedDatasetService>();
            mock.Setup(s => s.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<IDictionary<string, string>>())).Verifiable();
            var service = mock.Object;

            var parser = new SharedDatasetParser(service);

            var xmlDoc = new XmlDocument();
            using (Stream stream = Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("RsPackage.Testing.Resources.SharedDatasetSample.xml"))
            using (StreamReader reader = new StreamReader(stream))
                xmlDoc.Load(reader);

            var root = xmlDoc.FirstChild.NextSibling.SelectSingleNode("./Folder");
            parser.Root = new ProjectParser() { NamingConvention = new TitleToCamelCase() };
            parser.Execute(root);

            Mock.Get(service).Verify(s => s.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<IDictionary<string, string>>()), Times.Exactly(4));
        }

        [Test]
        public void SharedDatasetNodeHidden()
        {
            var mock = new Mock<SharedDatasetService>();
            mock.Setup(s => s.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<IDictionary<string, string>>())).Verifiable();
            var service = mock.Object;

            var parser = new SharedDatasetParser(service);

            var xmlDoc = new XmlDocument();
            using (Stream stream = Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("RsPackage.Testing.Resources.SharedDatasetSample.xml"))
            using (StreamReader reader = new StreamReader(stream))
                xmlDoc.Load(reader);

            var root = xmlDoc.FirstChild.NextSibling.SelectSingleNode("./Folder");
            parser.Root = new ProjectParser() { NamingConvention = new TitleToCamelCase() };
            parser.Execute(root);

            Mock.Get(service).Verify(s => s.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), true, It.IsAny<string>(), It.IsAny<IDictionary<string, string>>()), Times.Once);
            Mock.Get(service).Verify(s => s.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), false, It.IsAny<string>(), It.IsAny<IDictionary<string, string>>()), Times.Exactly(3));
        }

        [Test]
        public void SharedDatasetNodeDescription()
        {
            var mock = new Mock<SharedDatasetService>();
            mock.Setup(s => s.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<IDictionary<string, string>>())).Verifiable();
            var service = mock.Object;

            var parser = new SharedDatasetParser(service);

            var xmlDoc = new XmlDocument();
            using (Stream stream = Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("RsPackage.Testing.Resources.SharedDatasetSample.xml"))
            using (StreamReader reader = new StreamReader(stream))
                xmlDoc.Load(reader);

            var root = xmlDoc.FirstChild.NextSibling.SelectSingleNode("./Folder");
            parser.Root = new ProjectParser() { NamingConvention = new TitleToCamelCase() };
            parser.Execute(root);

            Mock.Get(service).Verify(s => s.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), "My description", It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<IDictionary<string, string>>()), Times.Once);
            Mock.Get(service).Verify(s => s.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), null, It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<IDictionary<string, string>>()), Times.Exactly(3));
        }
        

        [Test]
        public void SharedDatasetNodeAndForwardToRoot()
        {
            var stub = new Mock<SharedDatasetService>();
            stub.Setup(s => s.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<IDictionary<string, string>>())).Verifiable();
            var service = stub.Object;

            var parser = new SharedDatasetParser(service);

            var xmlDoc = new XmlDocument();
            using (Stream stream = Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("RsPackage.Testing.Resources.SharedDatasetSample.xml"))
            using (StreamReader reader = new StreamReader(stream))
                xmlDoc.Load(reader);

            var root = xmlDoc.FirstChild.NextSibling.SelectSingleNode("./Folder");
            parser.Root = new ProjectParser() { NamingConvention = new TitleToCamelCase() };
            parser.ParentPath = "/Shared";
            parser.Execute(root);

            Assert.That(parser.Root.SharedDatasets.ContainsKey("EmployeeSalesDetail"), Is.True);
            Assert.That(parser.Root.SharedDatasets["EmployeeSalesDetail"], Is.EqualTo("/Shared/EmployeeSalesDetail"));
        }

    }
}
