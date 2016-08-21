using NUnit.Framework;
using RsPackage.Factory;
using RsPackage.Execution;
using RsPackage.CommandLineArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RsPackage.Testing.Factory
{
    [TestFixture]
    public class ServiceBuilderTest
    {
        [Test]
        public void GetReportService_FullName()
        {
            var options = new PublishOptions() { Url = "reporting.company.com", LogPath="C:\\"};

            var serviceBuilder = new ServiceBuilder();
            serviceBuilder.Setup(options);
            serviceBuilder.Build();
            var service = serviceBuilder.GetReportService();

            Assert.That(service, Is.TypeOf<ReportService>());
            Assert.That(service, Is.Not.Null);
        }

        [Test]
        public void GetFolderService_FullName()
        {
            var options = new PublishOptions() { Url = "reporting.company.com", LogPath = "C:\\" };

            var serviceBuilder = new ServiceBuilder();
            serviceBuilder.Setup(options);
            serviceBuilder.Build();
            var service = serviceBuilder.GetFolderService();

            Assert.That(service, Is.TypeOf<FolderService>());
            Assert.That(service, Is.Not.Null);
        }

        [Test]
        public void GetDataSourceService_FullName()
        {
            var options = new PublishOptions() { Url = "reporting.company.com", LogPath = "C:\\" };

            var serviceBuilder = new ServiceBuilder();
            serviceBuilder.Setup(options);
            serviceBuilder.Build();
            var service = serviceBuilder.GetDataSourceService();

            Assert.That(service, Is.TypeOf<DataSourceService>());
            Assert.That(service, Is.Not.Null);
        }

        [Test]
        public void GetPolicyService_FullName()
        {
            var options = new PublishOptions() { Url = "reporting.company.com", LogPath = "C:\\" };

            var serviceBuilder = new ServiceBuilder();
            serviceBuilder.Setup(options);
            serviceBuilder.Build();
            var service = serviceBuilder.GetPolicyService();

            Assert.That(service, Is.TypeOf<PolicyService>());
            Assert.That(service, Is.Not.Null);
        }
    }
}
