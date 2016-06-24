﻿using NUnit.Framework;
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
    public class ReportParserTest
    {
        [Test]
        public void ParseReportNode()
        {
            var mock = new Mock<ReportService>();
            mock.Setup(s => s.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Verifiable();
            var service = mock.Object;

            var parser = new ReportParser(service);

            var xmlDoc = new XmlDocument();
            using (Stream stream = Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("RsDeploy.Testing.Resources.BasicSample.xml"))
            using (StreamReader reader = new StreamReader(stream))
                xmlDoc.Load(reader);

            var root = xmlDoc.FirstChild.NextSibling.SelectSingleNode("./Folder[@Name='Analysis']");
            parser.Execute(root, "parent");

            Mock.Get(service).Verify(s => s.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void ParseTwoReportNodes()
        {
            var mock = new Mock<ReportService>();
            mock.Setup(s => s.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Verifiable();
            var service = mock.Object;

            var parser = new ReportParser(service);

            var xmlDoc = new XmlDocument();
            using (Stream stream = Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("RsDeploy.Testing.Resources.BasicSample.xml"))
            using (StreamReader reader = new StreamReader(stream))
                xmlDoc.Load(reader);

            var root = xmlDoc.FirstChild.NextSibling;
            parser.Execute(root, "parent");

            Mock.Get(service).Verify(s => s.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
        }

        [Test]
        public void ParseCorrectName()
        {
            var mock = new Mock<ReportService>();
            mock.Setup(s => s.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Verifiable();
            var service = mock.Object;

            var parser = new ReportParser(service);

            var xmlDoc = new XmlDocument();
            using (Stream stream = Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("RsDeploy.Testing.Resources.BasicSample.xml"))
            using (StreamReader reader = new StreamReader(stream))
                xmlDoc.Load(reader);

            var root = xmlDoc.FirstChild.NextSibling.SelectSingleNode("./Folder[@Name='Analysis']");
            parser.Execute(root, "parent");

            Mock.Get(service).Verify(s => s.Create("Company sales", It.IsAny<string>(), It.IsAny<string>()));
        }

        [Test]
        public void ParseCorrectParent()
        {
            var mock = new Mock<ReportService>();
            mock.Setup(s => s.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Verifiable();
            var service = mock.Object;

            var parser = new ReportParser(service);

            var xmlDoc = new XmlDocument();
            using (Stream stream = Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("RsDeploy.Testing.Resources.BasicSample.xml"))
            using (StreamReader reader = new StreamReader(stream))
                xmlDoc.Load(reader);

            var root = xmlDoc.FirstChild.NextSibling.SelectSingleNode("./Folder[@Name='Analysis']");
            parser.Execute(root, "parent");

            Mock.Get(service).Verify(s => s.Create(It.IsAny<string>(),"parent", It.IsAny<string>()));
        }

        [Test]
        public void ParseCorrectPathWhenUnspecified()
        {
            var mock = new Mock<ReportService>();
            mock.Setup(s => s.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Verifiable();
            var service = mock.Object;

            var parser = new ReportParser(service);

            var xmlDoc = new XmlDocument();
            using (Stream stream = Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("RsDeploy.Testing.Resources.BasicSample.xml"))
            using (StreamReader reader = new StreamReader(stream))
                xmlDoc.Load(reader);

            var root = xmlDoc.FirstChild.NextSibling;
            parser.Execute(root, "parent");

            Mock.Get(service).Verify(s => s.Create(It.IsAny<string>(), It.IsAny<string>(), "DepartmentSales.rdl"));
        }
    }
}