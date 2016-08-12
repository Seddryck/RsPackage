using NUnit.Framework;
using SsrsDeploy.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SsrsDeploy.Testing
{
    [TestFixture]
    public class UrlBuilderTest
    {
        [Test]
        public void GetUrl_FullName()
        {
            var options = new Options() { Url = "http://reporting.company.com/ReportServer/ReportService2010.asmx" };
            var urlBuilder = new UrlBuilder();
            urlBuilder.Setup(options);
            urlBuilder.Build();
            var url = urlBuilder.GetUrl();

            Assert.That(url, Is.EqualTo("http://reporting.company.com/ReportServer/ReportService2010.asmx"));
        }

        [Test]
        public void GetUrl_Host()
        {
            var options = new Options() { Url = "reporting.company.com" };
            var urlBuilder = new UrlBuilder();
            urlBuilder.Setup(options);
            urlBuilder.Build();
            var url = urlBuilder.GetUrl();

            Assert.That(url, Is.EqualTo("http://reporting.company.com/ReportServer/ReportService2010.asmx"));
        }

        [Test]
        public void GetUrl_SchemeHost()
        {
            var options = new Options() { Url = "http://reporting.company.com" };
            var urlBuilder = new UrlBuilder();
            urlBuilder.Setup(options);
            urlBuilder.Build();
            var url = urlBuilder.GetUrl();

            Assert.That(url, Is.EqualTo("http://reporting.company.com/ReportServer/ReportService2010.asmx"));
        }

        [Test]
        public void GetUrl_HostPath()
        {
            var options = new Options() { Url = "reporting.company.com/ReportServer" };
            var urlBuilder = new UrlBuilder();
            urlBuilder.Setup(options);
            urlBuilder.Build();
            var url = urlBuilder.GetUrl();

            Assert.That(url, Is.EqualTo("http://reporting.company.com/ReportServer/ReportService2010.asmx"));
        }

        [Test]
        public void GetUrl_HostPathSlash()
        {
            var options = new Options() { Url = "reporting.company.com/ReportServer/" };
            var urlBuilder = new UrlBuilder();
            urlBuilder.Setup(options);
            urlBuilder.Build();
            var url = urlBuilder.GetUrl();

            Assert.That(url, Is.EqualTo("http://reporting.company.com/ReportServer/ReportService2010.asmx"));
        }

        [Test]
        public void GetUrl_HostPathFile()
        {
            var options = new Options() { Url = "reporting.company.com/ReportServer/ReportService2010.asmx" };
            var urlBuilder = new UrlBuilder();
            urlBuilder.Setup(options);
            urlBuilder.Build();
            var url = urlBuilder.GetUrl();

            Assert.That(url, Is.EqualTo("http://reporting.company.com/ReportServer/ReportService2010.asmx"));
        }

        [Test]
        public void GetUrl_SchemeHostPort()
        {
            var options = new Options() { Url = "http://reporting.company.com:8080" };
            var urlBuilder = new UrlBuilder();
            urlBuilder.Setup(options);
            urlBuilder.Build();
            var url = urlBuilder.GetUrl();

            Assert.That(url, Is.EqualTo("http://reporting.company.com:8080/ReportServer/ReportService2010.asmx"));
        }
    }
}
