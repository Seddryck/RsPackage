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
    public class PolicyServiceTest
    {
        private string ProductCatalogPath { get; set; }

        protected ReportingService.ReportingService2010 GetReportingService()
        {
            var rs = new ReportingService.ReportingService2010();
            rs.Url = "http://localhost/ReportServer_SQL2014/ReportService2010.asmx";
            rs.Credentials = System.Net.CredentialCache.DefaultCredentials;
            return rs;
        }

        [SetUp]
        public void SetUp()
        {
            TearDown();

            var rs = GetReportingService();
            rs.CreateFolder("ReportFolder", "/", null);
        }

        [TearDown]
        public void TearDown()
        {
            var rs = GetReportingService();

            if (rs.GetItemType("/ReportFolder") == "Folder")
                rs.DeleteItem("/ReportFolder");

            var testRoles = rs.ListRoles("All", null).Where(r => r.Name.StartsWith("*Test*")).Select(r => r.Name);
            foreach (var testRole in testRoles)
                rs.DeleteRole(testRole);
        }

        [Test]
        public void CreateNewPolicy()
        {
            var rs = GetReportingService();
            rs.CreateRole("*Test* My First Role", "My First Role description", new[] { "Manage all subscriptions", "View reports" });

            var service = new PolicyService(rs);
            var newPolicy = new Tuple<string, string[]>("*Test* My First Role", new[] { @"BELGRID\GHL255" });
            service.Create("/ReportFolder", Enumerable.Repeat(newPolicy, 1));

            var inherit = true;
            var policies = rs.GetPolicies("*Test* My First Role", out inherit);

            Assert.That(inherit, Is.False);
            Assert.That(policies, Has.Count.EqualTo(1));
        }

        [Test]
        public void CreateExistingRoleAndUpdateItByRemovingTasks()
        {
            var rs = GetReportingService();
            rs.CreateRole("*Test* My First Role", "My First Role description", new[] { "Manage all subscriptions", "View reports" });

            var service = new RoleService(rs);
            service.Create("*Test* My First Role", "My First Role description 2", new[] { "Manage all subscriptions" });

            var descr = string.Empty;
            var tasks = rs.GetRoleProperties("*Test* My First Role", null, out descr);

            Assert.That(tasks, Has.Count.EqualTo(1));
            Assert.That(descr, Is.EqualTo("My First Role description 2"));
        }
    }
}
