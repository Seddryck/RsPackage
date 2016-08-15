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
    public class PolicyParserTest
    {
        [Test]
        public void ParsePolicyNode()
        {
            var mock = new Mock<PolicyService>();
            mock.Setup(s => s.Create(It.IsAny<string>(), It.IsAny<IEnumerable<Tuple<string, string[]>>>())).Verifiable();
            var service = mock.Object;

            var parser = new PolicyParser(service);

            var xmlDoc = new XmlDocument();
            using (Stream stream = Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("SsrsDeploy.Testing.Resources.BasicSample.xml"))
            using (StreamReader reader = new StreamReader(stream))
                xmlDoc.Load(reader);

            var root = xmlDoc.FirstChild.NextSibling.SelectSingleNode("./Report[@Name='Team sales']");
            parser.Execute(root);

            Mock.Get(service).Verify(s => s.Create(It.IsAny<string>(), It.IsAny<IEnumerable<Tuple<string, string[]>>>()), Times.Once);
        }

        [Test]
        public void ParseCorrectMembership()
        {
            var mock = new Mock<PolicyService>();
            mock.Setup(s => s.Create(It.IsAny<string>(), It.IsAny<IEnumerable<Tuple<string, string[]>>>())).Verifiable();
            var service = mock.Object;

            var parser = new PolicyParser(service);

            var xmlDoc = new XmlDocument();
            using (Stream stream = Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("SsrsDeploy.Testing.Resources.BasicSample.xml"))
            using (StreamReader reader = new StreamReader(stream))
                xmlDoc.Load(reader);

            var root = xmlDoc.FirstChild.NextSibling.SelectSingleNode("./Report[@Name='Team sales']");
            parser.Execute(root);

            Mock.Get(service).Verify(s => s.Create(It.IsAny<string>(), It.Is<IEnumerable<Tuple<string, string[]>>>(r => r.Count() == 1)), Times.Once);
        }

        [Test]
        public void ParseCorrectRole()
        {
            var mock = new Mock<PolicyService>();
            mock.Setup(s => s.Create(It.IsAny<string>(), It.IsAny<IEnumerable<Tuple<string, string[]>>>())).Verifiable();
            var service = mock.Object;

            var parser = new PolicyParser(service);

            var xmlDoc = new XmlDocument();
            using (Stream stream = Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("SsrsDeploy.Testing.Resources.BasicSample.xml"))
            using (StreamReader reader = new StreamReader(stream))
                xmlDoc.Load(reader);

            var root = xmlDoc.FirstChild.NextSibling.SelectSingleNode("./Report[@Name='Team sales']");
            parser.Execute(root);

            Mock.Get(service).Verify(s => s.Create(It.IsAny<string>(), It.Is<IEnumerable<Tuple<string, string[]>>>(r => r.ElementAt(0).Item1=="Browser")), Times.Once);
        }

        [Test]
        public void ParseCorrectMembers()
        {
            var mock = new Mock<PolicyService>();
            mock.Setup(s => s.Create(It.IsAny<string>(), It.IsAny<IEnumerable<Tuple<string, string[]>>>())).Verifiable();
            var service = mock.Object;

            var parser = new PolicyParser(service);

            var xmlDoc = new XmlDocument();
            using (Stream stream = Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("SsrsDeploy.Testing.Resources.BasicSample.xml"))
            using (StreamReader reader = new StreamReader(stream))
                xmlDoc.Load(reader);

            var root = xmlDoc.FirstChild.NextSibling.SelectSingleNode("./Report[@Name='Team sales']");
            parser.Execute(root);

            Mock.Get(service).Verify(s => s.Create(It.IsAny<string>(), It.Is<IEnumerable<Tuple<string, string[]>>>(
                r => r.ElementAt(0).Item2.Contains("Domain\\User1")
                && r.ElementAt(0).Item2.Contains("Domain\\User2")
                && r.ElementAt(0).Item2.Count()==2))
                , Times.Once);
        }

        [Test]
        public void ParseCorrectParent()
        {
            var mock = new Mock<PolicyService>();
            mock.Setup(s => s.Create(It.IsAny<string>(), It.IsAny<IEnumerable<Tuple<string, string[]>>>())).Verifiable();
            var service = mock.Object;

            var parser = new PolicyParser(service);
            parser.ParentPath = "/Root/First-Child";

            var xmlDoc = new XmlDocument();
            using (Stream stream = Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("SsrsDeploy.Testing.Resources.BasicSample.xml"))
            using (StreamReader reader = new StreamReader(stream))
                xmlDoc.Load(reader);

            var root = xmlDoc.FirstChild.NextSibling.SelectSingleNode("./Report[@Name='Team sales']");
            parser.Execute(root);

            Mock.Get(service).Verify(s => s.Create("/Root/First-Child", It.IsAny<IEnumerable<Tuple<string, string[]>>>()), Times.Once);
        }
    }
}
