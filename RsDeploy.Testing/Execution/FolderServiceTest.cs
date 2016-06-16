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
    public class FolderServiceTest
    {
        protected ReportingService.ReportingService2010 GetReportingService()
        {
            var rs = new ReportingService.ReportingService2010();
            rs.Credentials = System.Net.CredentialCache.DefaultCredentials;
            return rs;
        }

        [SetUp]
        public void SetUp()
        {
            Cleanup();
        }

        [TearDown]
        public void TearDown()
        {
            Cleanup();
        }

        private void Cleanup()
        {
            var rs = GetReportingService();

            if (rs.GetItemType("/ChildFolder") == "Folder")
                rs.DeleteItem("/ChildFolder");

            if (rs.GetItemType("/FirstFolder") == "Folder")
                rs.DeleteItem("/FirstFolder");
        }

        [Test]
        public void CreateFolderAtRootLevel()
        {
            var rs = GetReportingService();

            var service = new FolderService(rs);
            service.Create("ChildFolder", "/");

            Assert.That(rs.GetItemType("/ChildFolder"), Is.EqualTo("Folder"));
        }

        [Test]
        public void CreateFolderAtSecondLevel()
        {
            var rs = GetReportingService();

            rs.CreateFolder("FirstFolder", "/", null);

            var service = new FolderService(rs);
            service.Create("SecondFolder", "/FirstFolder");

            Assert.That(rs.GetItemType("/FirstFolder/SecondFolder"), Is.EqualTo("Folder"));
        }

        [Test]
        public void CreateAlreadyExistingFolder()
        {
            var rs = GetReportingService();

            rs.CreateFolder("ChildFolder", "/", null);

            var service = new FolderService(rs);
            service.Create("ChildFolder", "/");

            Assert.That(rs.GetItemType("/ChildFolder"), Is.EqualTo("Folder"));
        }
    }
}
