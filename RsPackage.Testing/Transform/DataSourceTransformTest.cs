using NUnit.Framework;
using RsPackage.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RsPackage.Testing.Transform
{
    [TestFixture]
    public class DataSourceTransformTest
    {
        [Test]
        public void ToReportingService_IntegratedSecurity()
        {
            var filename = FileOnDisk.CreatePhysicalFile("AdventureWorks_Integrated.rds", "RsPackage.Testing.Resources.AdventureWorks.rds");

            var visualStudioDoc = new XmlDocument();
            visualStudioDoc.Load(filename);

            var transformer = new DataSourceTransformer();
            var reportingServiceDoc = transformer.ToReportingService(visualStudioDoc);

            Assert.That(reportingServiceDoc.FirstChild.NodeType, Is.EqualTo(XmlNodeType.XmlDeclaration));
            Assert.That(reportingServiceDoc.ChildNodes.Count, Is.EqualTo(2));
            Assert.That(reportingServiceDoc.LastChild.NodeType, Is.EqualTo(XmlNodeType.Element));
            Assert.That(reportingServiceDoc.LastChild.ChildNodes.Count, Is.EqualTo(4));
            Assert.That(reportingServiceDoc.LastChild.ChildNodes[0].Name, Is.EqualTo("Extension"));
            Assert.That(reportingServiceDoc.LastChild.ChildNodes[0].InnerText, Is.EqualTo("SQL"));
            Assert.That(reportingServiceDoc.LastChild.ChildNodes[1].Name, Is.EqualTo("ConnectString"));
            Assert.That(reportingServiceDoc.LastChild.ChildNodes[2].Name, Is.EqualTo("CredentialRetrieval"));
            Assert.That(reportingServiceDoc.LastChild.ChildNodes[2].InnerText, Is.EqualTo("Integrated"));
            Assert.That(reportingServiceDoc.LastChild.ChildNodes[3].Name, Is.EqualTo("Enabled"));
            Assert.That(reportingServiceDoc.LastChild.ChildNodes[3].InnerText, Is.EqualTo("True"));
        }
    }
}
