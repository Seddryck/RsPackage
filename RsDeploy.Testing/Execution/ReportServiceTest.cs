using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services;
using RsDeploy.Execution;
using RsDeploy.ReportingService;

namespace RsDeploy.Testing.Execution
{
    [TestFixture]
    public class ReportServiceTest
    {
        private string ProductCatalogPath { get; set; }

        protected ReportingService.ReportingService2010 GetReportingService()
        {
            var rs = new ReportingService.ReportingService2010();
            rs.Url = ConnectionStringReader.GetReportingServiceUrl();
            rs.Credentials = System.Net.CredentialCache.DefaultCredentials;
            return rs;
        }

        [SetUp]
        public void SetUp()
        {
            var rs = GetReportingService();

            if (rs.GetItemType("/ReportFolder") == "Folder")
                rs.DeleteItem("/ReportFolder");

            rs.CreateFolder("ReportFolder", "/", null);

            ProductCatalogPath = FileOnDisk.CreatePhysicalFile("ProductCatalog.rdl", "RsDeploy.Testing.Resources.Product Catalog.rdl");
        }

        [TearDown]
        public void TearDown()
        {
            var rs = GetReportingService();

            if (rs.GetItemType("/ReportFolder") == "Folder")
                rs.DeleteItem("/ReportFolder");
        }

        [Test]
        public void CreateReport()
        {
            var rs = GetReportingService();

            var service = new ReportService(rs);
            service.Create("My First Report", "/ReportFolder", ProductCatalogPath);

            Assert.That(rs.GetItemType("/ReportFolder/My First Report"), Is.EqualTo("Report"));

        }

        [Test]
        public void CreateVisibleReport()
        {
            var rs = GetReportingService();

            var service = new ReportService(rs);
            service.Create("My First Report", "/ReportFolder", ProductCatalogPath, "My description", false);

            Assert.That(rs.GetItemType("/ReportFolder/My First Report"), Is.EqualTo("Report"));
            var properties = rs.GetProperties("/ReportFolder/My First Report", new[] { new Property() { Name = "Hidden" } });
            Assert.That(properties[0].Value, Is.EqualTo(false.ToString()));
        }

        [Test]
        public void CreateReportWithDescription()
        {
            var rs = GetReportingService();

            var service = new ReportService(rs);
            service.Create("My First Report", "/ReportFolder", ProductCatalogPath, "My description", false);

            Assert.That(rs.GetItemType("/ReportFolder/My First Report"), Is.EqualTo("Report"));
            var properties = rs.GetProperties("/ReportFolder/My First Report", new[] { new Property() { Name = "Description" } });
            Assert.That(properties[0].Value, Is.EqualTo("My description"));
        }

        [Test]
        public void CreateHiddenReport()
        {
            var rs = GetReportingService();

            var service = new ReportService(rs);
            service.Create("My First Report", "/ReportFolder", ProductCatalogPath, "My description", true);

            Assert.That(rs.GetItemType("/ReportFolder/My First Report"), Is.EqualTo("Report"));
            var properties = rs.GetProperties("/ReportFolder/My First Report", new[] { new Property(){ Name = "Hidden" }});
            Assert.That(properties[0].Value, Is.EqualTo(true.ToString()));
        }

    }
}
