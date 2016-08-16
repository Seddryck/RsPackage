using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services;
using RsPackage.Execution;

namespace RsPackage.Testing.Execution
{
    [TestFixture]
    [Category("NotAdvancedServices ")]
    public class RoleServiceTest
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
            TearDown();
        }

        [TearDown]
        public void TearDown()
        {
            var rs = GetReportingService();

            var testRoles = rs.ListRoles("All", null).Where(r => r.Name.StartsWith("*Test*")).Select(r => r.Name);
            foreach (var testRole in testRoles)
                rs.DeleteRole(testRole);
        }

        [Test]
        public void CreateNewRole()
        {
            var rs = GetReportingService();
            
            var service = new RoleService(rs);
            service.Create("*Test* My First Role", "My First Role description", new[] {"Manage all subscriptions", "View reports"});

            var descr = string.Empty;
            var tasks = rs.GetRoleProperties("*Test* My First Role", null, out descr);

            Assert.That(tasks.Count, Is.EqualTo(2));
            Assert.That(descr, Is.EqualTo("My First Role description"));
        }

        [Test]
        public void CreateExistingRole()
        {
            var rs = GetReportingService();
            var taskIDs = new List<string>();
            taskIDs.AddRange(
                rs.ListTasks("All")
                    .Where(t => new[] { "Manage all subscriptions", "View reports" }.Contains(t.Name))
                    .Select(t => t.TaskID)
            );

            rs.CreateRole("*Test* My First Role", "My First Role description", taskIDs.ToArray());

            var service = new RoleService(rs);
            service.Create("*Test* My First Role", "My First Role description", new[] { "Manage all subscriptions", "View reports" });

            var descr = string.Empty;
            var tasks = rs.GetRoleProperties("*Test* My First Role", null, out descr);

            Assert.That(tasks.Count(), Is.EqualTo(2));
            Assert.That(descr, Is.EqualTo("My First Role description"));
        }

        [Test]
        public void CreateExistingRoleAndUpdateItByAddingTasks()
        {
            var rs = GetReportingService();
            var taskIDs = new List<string>();
            taskIDs.AddRange(
                rs.ListTasks("All")
                    .Where(t => new[] { "Manage all subscriptions", "View reports" }.Contains(t.Name))
                    .Select(t => t.TaskID)
            );

            rs.CreateRole("*Test* My First Role", "My First Role description", taskIDs.Take(1).ToArray());

            var service = new RoleService(rs);
            service.Create("*Test* My First Role", "My First Role description 2", new[] { "Manage all subscriptions", "View reports" });

            var descr = string.Empty;
            var tasks = rs.GetRoleProperties("*Test* My First Role", null, out descr);

            Assert.That(tasks.Count, Is.EqualTo(2));
            Assert.That(descr, Is.EqualTo("My First Role description 2"));
        }

        [Test]
        public void CreateExistingRoleAndUpdateItByRemovingTasks()
        {
            var rs = GetReportingService();
            var taskIDs = new List<string>();
            taskIDs.AddRange(
                rs.ListTasks("All")
                    .Where(t => new[] { "Manage all subscriptions", "View reports" }.Contains(t.Name))
                    .Select(t => t.TaskID)
            );

            rs.CreateRole("*Test* My First Role", "My First Role description", taskIDs.ToArray());

            var service = new RoleService(rs);
            service.Create("*Test* My First Role", "My First Role description 2", new[] { "Manage all subscriptions" });

            var descr = string.Empty;
            var tasks = rs.GetRoleProperties("*Test* My First Role", null, out descr);

            Assert.That(tasks.Count, Is.EqualTo(1));
            Assert.That(descr, Is.EqualTo("My First Role description 2"));
        }

    }
}
