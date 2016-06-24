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

            if (rs.GetItemType("/Data Sources") != "Folder")
                rs.CreateFolder("Data Sources", "/", null);

            if (rs.GetItemType("/Data Sources/AdventureWorks") != "DataSource")
            {
                var ds = new DataSourceDefinition()
                {
                    ConnectString = "Data Source=localhost;Initial Catalog=msdb;Integreted Security=SSPI"
                };
                rs.CreateDataSource("AdventureWorks", "/Data Sources", false, ds, null);
            }
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
            var properties = rs.GetProperties("/ReportFolder/My First Report", new[] { new Property() { Name = "Hidden" } });
            Assert.That(properties[0].Value, Is.EqualTo(true.ToString()));
        }

        [Test]
        public void CreateReportRedirectDataSource()
        {
            var rs = GetReportingService();

            var ds = new Dictionary<string, string>();
            ds.Add("AdventureWorks", "/Data Sources/AdventureWorks");

            var service = new ReportService(rs);
            service.Create("My First Report", "/ReportFolder", ProductCatalogPath, "My description", false, ds);

            var dsRef = rs.GetItemDataSources("/ReportFolder/My First Report");
            Assert.That(dsRef.Count(), Is.EqualTo(1));
            Assert.That(dsRef[0].Item, Is.TypeOf<DataSourceReference>());
            Assert.That((dsRef[0].Item as DataSourceReference).Reference, Is.EqualTo("/Data Sources/AdventureWorks"));
        }

        [Test]
        public void CreateReportRedirectMissingDataSource()
        {
            var rs = GetReportingService();

            var ds = new Dictionary<string, string>();

            var service = new ReportService(rs);
            var error = false;
            service.MessageSent += (o, e) => error |= e.Level == MessageEventArgs.LevelOption.Error;

            Assert.Catch<InvalidOperationException>(() => service.Create("My First Report", "/ReportFolder", ProductCatalogPath, "My description", false, ds));
            Assert.That(error, Is.True);
        }

    }
}
