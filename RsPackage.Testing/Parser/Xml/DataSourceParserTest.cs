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
using RsPackage.Action;

namespace RsPackage.Testing.Parser.Xml
{
    [TestFixture]
    public class DataSourceParserTest
    {
        [Test]
        public void ParseDataSourceNode()
        {
            var mock = new Mock<DataSourceService>();
            mock.Setup(s => s.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Verifiable();
            var service = mock.Object;

            var parser = new DataSourceParser(service);

            var xmlDoc = new XmlDocument();
            using (Stream stream = Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("RsPackage.Testing.Resources.BasicSample.xml"))
            using (StreamReader reader = new StreamReader(stream))
                xmlDoc.Load(reader);

            var root = xmlDoc.FirstChild.NextSibling;
            parser.Root = new MultipleFilesPublisher() { NamingConvention = new TitleToCamelCase() };
            parser.Execute(root);

            Mock.Get(service).Verify(s => s.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void ParseDataSourceNodeAndForwardToRoot()
        {
            var stub = new Mock<DataSourceService>();
            stub.Setup(s => s.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Verifiable();
            var service = stub.Object;

            var parser = new DataSourceParser(service);

            var xmlDoc = new XmlDocument();
            using (Stream stream = Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("RsPackage.Testing.Resources.BasicSample.xml"))
            using (StreamReader reader = new StreamReader(stream))
                xmlDoc.Load(reader);

            var root = xmlDoc.FirstChild.NextSibling;
            parser.Root = new MultipleFilesPublisher() { NamingConvention = new TitleToCamelCase() };
            parser.Execute(root);

            Assert.That(parser.Root.DataSources.ContainsKey("MyDataSource"), Is.True);
            Assert.That(parser.Root.DataSources["MyDataSource"], Is.EqualTo("/MyDataSource"));
        }

    }
}
