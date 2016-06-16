﻿using NUnit.Framework;
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
    public class RoleServiceTest
    {
        private string ProductCatalogPath { get; set; }

        protected ReportingService.ReportingService2010 GetReportingService()
        {
            var rs = new ReportingService.ReportingService2010();
            rs.Url = "http://localhost/ReportServer/ReportService2010.asmx";
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

            Assert.That(tasks, Has.Count.EqualTo(2));
            Assert.That(descr, Is.EqualTo("My First Role description"));
        }

        [Test]
        public void CreateExistingRole()
        {
            var rs = GetReportingService();
            rs.CreateRole("*Test* My First Role", "My First Role description", new[] { "Manage all subscriptions", "View reports" });

            var service = new RoleService(rs);
            service.Create("*Test* My First Role", "My First Role description", new[] { "Manage all subscriptions", "View reports" });

            var descr = string.Empty;
            var tasks = rs.GetRoleProperties("*Test* My First Role", null, out descr);

            Assert.That(tasks, Has.Count.EqualTo(2));
            Assert.That(descr, Is.EqualTo("My First Role description"));
        }

        [Test]
        public void CreateExistingRoleAndUpdateItByAddingTasks()
        {
            var rs = GetReportingService();
            rs.CreateRole("*Test* My First Role", "My First Role description", new[] { "Manage all subscriptions" });

            var service = new RoleService(rs);
            service.Create("*Test* My First Role", "My First Role description 2", new[] { "Manage all subscriptions", "View reports" });

            var descr = string.Empty;
            var tasks = rs.GetRoleProperties("*Test* My First Role", null, out descr);

            Assert.That(tasks, Has.Count.EqualTo(2));
            Assert.That(descr, Is.EqualTo("My First Role description 2"));
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
