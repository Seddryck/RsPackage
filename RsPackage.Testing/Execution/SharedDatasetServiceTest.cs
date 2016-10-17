using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services;
using System.IO;
using RsPackage.Execution;
using RsPackage.ReportingService;
using RsPackage.StreamProvider;

namespace RsPackage.Testing.Execution
{
    [TestFixture]
    [Category("NotAdvancedServices ")]
    public class SharedDatasetServiceTest
    {
        private string SharedDatasetPath { get; set; }
        private string[] SharedDatasetPaths { get; set; }

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

            if (rs.GetItemType("/SharedDatasetFolder") == "Folder")
                rs.DeleteItem("/SharedDatasetFolder");

            rs.CreateFolder("SharedDatasetFolder", "/", null);

            SharedDatasetPath = FileOnDisk.CreatePhysicalFile("Employee_Sales_Summary.rdl", "RsPackage.Testing.Resources.Employee_Sales_Summary.rdl");

            SharedDatasetPaths = new string[4];
            SharedDatasetPaths[0] = FileOnDisk.CreatePhysicalFile("EmployeeSalesDetail.rsd", "RsPackage.Testing.Resources.EmployeeSalesDetail.rsd");
            SharedDatasetPaths[1] = FileOnDisk.CreatePhysicalFile("EmployeeSalesYearOverYear.rsd", "RsPackage.Testing.Resources.EmployeeSalesYearOverYear.rsd");
            SharedDatasetPaths[2] = FileOnDisk.CreatePhysicalFile("EmpSalesMonth.rsd", "RsPackage.Testing.Resources.EmpSalesMonth.rsd");
            SharedDatasetPaths[3] = FileOnDisk.CreatePhysicalFile("SalesEmployees.rsd", "RsPackage.Testing.Resources.SalesEmployees.rsd");

            if (rs.GetItemType("/Data Sources") != "Folder")
                rs.CreateFolder("Data Sources", "/", null);

            if (rs.GetItemType("/Data Sources/AdventureWorks") != "DataSource")
            {
                var ds = new DataSourceDefinition()
                {
                    ConnectString = "Data Source=localhost;Initial Catalog=msdb;Integreted Security=SSPI",
                    Extension="SQL"
                };
                rs.CreateDataSource("AdventureWorks", "/Data Sources", false, ds, null);
            }
        }

        [TearDown]
        public void TearDown()
        {
            var rs = GetReportingService();

            if (rs.GetItemType("/SharedFolder") == "Folder")
                rs.DeleteItem("/SharedDatasetFolder");
        }

        [Test]
        public void CreateSharedDataset()
        {
            var rs = GetReportingService();

            var service = new SharedDatasetService(rs, new FileStreamProvider(Path.GetDirectoryName(SharedDatasetPaths[0])));
            service.Create("EmployeeSalesDetail", "/SharedDatasetFolder", Path.GetFileName(SharedDatasetPaths[0]), string.Empty, false);

            Assert.That(rs.GetItemType("/SharedDatasetFolder/EmployeeSalesDetail"), Is.EqualTo("DataSet"));

        }

        [Test]
        public void CreateVisibleSharedDataset()
        {
            var rs = GetReportingService();

            var service = new SharedDatasetService(rs, new FileStreamProvider(Path.GetDirectoryName(SharedDatasetPaths[0])));
            service.Create("EmployeeSalesDetail", "/SharedDatasetFolder", Path.GetFileName(SharedDatasetPaths[0]), "My description", false);

            Assert.That(rs.GetItemType("/SharedDatasetFolder/EmployeeSalesDetail"), Is.EqualTo("DataSet"));
            var properties = rs.GetProperties("/SharedDatasetFolder/EmployeeSalesDetail", new[] { new Property() { Name = "Hidden" } });
            Assert.That(properties[0].Value, Is.EqualTo(false.ToString()));
        }

        [Test]
        public void CreateHiddenSharedDataset()
        {
            var rs = GetReportingService();

            var service = new SharedDatasetService(rs, new FileStreamProvider(Path.GetDirectoryName(SharedDatasetPaths[0])));
            service.Create("EmployeeSalesDetail", "/SharedDatasetFolder", Path.GetFileName(SharedDatasetPaths[0]), string.Empty, true);

            Assert.That(rs.GetItemType("/SharedDatasetFolder/EmployeeSalesDetail"), Is.EqualTo("DataSet"));
            var properties = rs.GetProperties("/SharedDatasetFolder/EmployeeSalesDetail", new[] { new Property() { Name = "Hidden" } });
            Assert.That(properties[0].Value, Is.EqualTo(true.ToString()));
        }


        [Test]
        public void CreateSharedDatasetWithDescription()
        {
            var rs = GetReportingService();

            var service = new SharedDatasetService(rs, new FileStreamProvider(Path.GetDirectoryName(SharedDatasetPaths[0])));
            service.Create("EmployeeSalesDetail", "/SharedDatasetFolder", Path.GetFileName(SharedDatasetPaths[0]), "My description", false);

            Assert.That(rs.GetItemType("/SharedDatasetFolder/EmployeeSalesDetail"), Is.EqualTo("DataSet"));
            var properties = rs.GetProperties("/SharedDatasetFolder/EmployeeSalesDetail", new[] { new Property() { Name = "Description" } });
            Assert.That(properties[0].Value, Is.EqualTo("My description"));
        }
        
        [Test]
        public void CreateSharedDatasetRedirectDataSource()
        {
            var rs = GetReportingService();

            var ds = new Dictionary<string, string>();
            ds.Add("AdventureWorks", "/Data Sources/AdventureWorks");

            var service = new SharedDatasetService(rs, new FileStreamProvider(Path.GetDirectoryName(SharedDatasetPaths[0])));
            service.Create("EmployeeSalesDetail", "/SharedDatasetFolder", Path.GetFileName(SharedDatasetPaths[0]), "My description", false, "AdventureWorks", ds);

            var dsRef = rs.GetItemDataSources("/SharedDatasetFolder/EmployeeSalesDetail");
            Assert.That(dsRef.Count(), Is.EqualTo(1));
            Assert.That(dsRef[0].Item, Is.TypeOf<DataSourceReference>());
            Assert.That((dsRef[0].Item as DataSourceReference).Reference, Is.EqualTo("/Data Sources/AdventureWorks"));
        }

        [Test]
        public void CreateSharedDatasetRedirectMissingDataSource()
        {
            var rs = GetReportingService();

            var ds = new Dictionary<string, string>();

            var service = new SharedDatasetService(rs, new FileStreamProvider(Path.GetDirectoryName(SharedDatasetPaths[0])));
            var error = false;
            service.MessageSent += (o, e) => error |= e.Level == MessageEventArgs.LevelOption.Error;

            Assert.Catch<InvalidOperationException>(() => service.Create("EmployeeSalesDetail", "/SharedDatasetFolder", Path.GetFileName(SharedDatasetPath), string.Empty, false, "AdventureWorks", ds));
            Assert.That(error, Is.True);
        }

    }
}
