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
            parser.Execute(root, "parent");

            Mock.Get(service).Verify(s => s.Create(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
        
    }
}
