using NUnit.Framework;
using RsPackage.CommandLineArgs;
using RsPackage.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RsPackage.Testing.Factory
{
    [TestFixture]
    public class PackagerFactoryTest
    {
        [Test]
        public void AllInfoProvided()
        {
            var options = new PackageOptions()
            {
                SourceFile = @"C:\Folder\source.xml",
                ResourcePath = @"C:\Folder\Resources\",
                TargetFile = @"C:\Folder\target.rspac"
            };

            var factory = new PackagerFactory();
            var packager = factory.GetPackager(options);

            Assert.That(packager.SolutionFile, Is.EqualTo(@"C:\Folder\source.xml"));
            Assert.That(packager.ResourcePath, Is.EqualTo(@"C:\Folder\Resources\"));
            Assert.That(packager.TargetFile, Is.EqualTo(@"C:\Folder\target.rspac"));
        }

        [Test]
        public void MinimalInfoProvided()
        {
            var options = new PackageOptions()
            {
                SourceFile = @"C:\Folder\source.xml",
            };

            var factory = new PackagerFactory();
            var packager = factory.GetPackager(options);

            Assert.That(packager.SolutionFile, Is.EqualTo(@"C:\Folder\source.xml"));
            Assert.That(packager.ResourcePath, Is.EqualTo(@"C:\Folder\"));
            Assert.That(packager.TargetFile, Is.EqualTo(@"C:\Folder\source.rspac"));
        }

        [Test]
        public void RelativePathForResourcesProvided()
        {
            var options = new PackageOptions()
            {
                SourceFile = @"C:\Folder\source.xml",
                ResourcePath=@"..\Resources"
            };

            var factory = new PackagerFactory();
            var packager = factory.GetPackager(options);

            Assert.That(packager.SolutionFile, Is.EqualTo(@"C:\Folder\source.xml"));
            Assert.That(packager.ResourcePath, Is.EqualTo(@"C:\Folder\..\Resources"));
            Assert.That(packager.TargetFile, Is.EqualTo(@"C:\Folder\source.rspac"));
        }

        [Test]
        public void RelativePathForTargetProvided()
        {
            var options = new PackageOptions()
            {
                SourceFile = @"C:\Folder\source.xml",
                TargetFile = @"..\Resources\"
            };

            var factory = new PackagerFactory();
            var packager = factory.GetPackager(options);

            Assert.That(packager.SolutionFile, Is.EqualTo(@"C:\Folder\source.xml"));
            Assert.That(packager.ResourcePath, Is.EqualTo(@"C:\Folder\"));
            Assert.That(packager.TargetFile, Is.EqualTo(@"C:\Folder\..\Resources\source.rspac"));
        }

        [Test]
        public void NameForTargetProvided()
        {
            var options = new PackageOptions()
            {
                SourceFile = @"C:\Folder\source.xml",
                TargetFile = @"MyReports"
            };

            var factory = new PackagerFactory();
            var packager = factory.GetPackager(options);

            Assert.That(packager.SolutionFile, Is.EqualTo(@"C:\Folder\source.xml"));
            Assert.That(packager.ResourcePath, Is.EqualTo(@"C:\Folder\"));
            Assert.That(packager.TargetFile, Is.EqualTo(@"C:\Folder\MyReports.rspac"));
        }
    }
}
