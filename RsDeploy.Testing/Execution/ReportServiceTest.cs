using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services;
using RsDeploy.Execution;

namespace RsDeploy.Testing.Execution
{
    [TestFixture]
    public class ReportServiceTest
    {
        private string ProductCatalogPath { get; set; }

        [SetUp]
        public void SetUp()
        {
            var rs = new ReportingService.ReportingService2010();
            rs.Credentials = System.Net.CredentialCache.DefaultCredentials;

            if (rs.GetItemType("/ReportFolder") == "Folder")
                rs.DeleteItem("/ReportFolder");

            rs.CreateFolder("ReportFolder", "/", null);

            ProductCatalogPath = FileOnDisk.CreatePhysicalFile("ProductCatalog.rdl", "RsDeploy.Testing.Resources.Product Catalog.rdl");
        }

        [TearDown]
        public void TearDown()
        {
            var rs = new ReportingService.ReportingService2010();
            rs.Credentials = System.Net.CredentialCache.DefaultCredentials;

            if (rs.GetItemType("/ReportFolder") == "Folder")
                rs.DeleteItem("/ReportFolder");
        }

        [Test]
        public void CreateReport()
        {
            var rs = new ReportingService.ReportingService2010();
            rs.Credentials = System.Net.CredentialCache.DefaultCredentials;   

            var service = new ReportService(rs);
            service.Create("My First Report", "/ReportFolder", ProductCatalogPath);

            Assert.That(rs.GetItemType("/ReportFolder/My First Report"), Is.EqualTo("Report"));
        }
        
    }
}
