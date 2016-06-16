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
    [Category("NotAdvancedServices ")]
    public class PolicyServiceTest
    {
        private string ProductCatalogPath { get; set; }

        protected ReportingService.ReportingService2010 GetReportingService()
        {
            var rs = new ReportingService.ReportingService2010();
            rs.Url = ConnectionStringReader.GetReportingServiceUrl();
            rs.Credentials = System.Net.CredentialCache.DefaultCredentials;
            return rs;
        }

        protected string GetUserName()
        {
            return System.Security.Principal.WindowsIdentity.GetCurrent().Name;
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
            var newPolicy = new Tuple<string, string[]>("*Test* My First Role", new[] { GetUserName() });
            service.Create("/ReportFolder", Enumerable.Repeat(newPolicy, 1));

            var inherit = true;
            var policies = rs.GetPolicies("*Test* My First Role", out inherit);

            Assert.That(inherit, Is.False);
            Assert.That(policies, Has.Count.EqualTo(1));
        }

        [Test]
        public void CreateNewPolicyWithTwoRoles()
        {
            var rs = GetReportingService();
            rs.CreateRole("*Test* My First Role", "My First Role description", new[] { "View reports" });
            rs.CreateRole("*Test* My Second Role", "My Second Role description", new[] { "Manage all subscriptions" });

            var service = new PolicyService(rs);
            var newPolicy1 = new Tuple<string, string[]>("*Test* My First Role", new[] { GetUserName() });
            var newPolicy2 = new Tuple<string, string[]>("*Test* My Second Role", new[] { GetUserName() });
            var newPolicies = new List<Tuple<string, string[]>>();
            newPolicies.Add(newPolicy1);
            newPolicies.Add(newPolicy2);
            service.Create("/ReportFolder", newPolicies);

            var inherit = true;
            var policies = rs.GetPolicies("*Test* My First Role", out inherit);

            Assert.That(inherit, Is.False);
            Assert.That(policies, Has.Count.EqualTo(1));
        }

    }
}
