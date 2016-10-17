using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using NUnit.Framework;
using RsPackage.StreamProvider;

namespace RsPackage.Testing.StreamProvider
{
    [TestFixture]
    public class ZipStreamProviderTest
    {

        private string zipFile;

        [SetUp]
        public void Setup()
        {
            zipFile = FileOnDisk.CreatePhysicalFile("MyPackage.rspac", "RsPackage.Testing.Resources.MyPackage.rspac");
        }

        [Test]
        public void Exists_ExistingFile_True()
        {
            var provider = new ZipStreamProvider(zipFile);
            var result = provider.Exists("EmployeeSalesDetail.rsd");
            Assert.IsTrue(result);
        }

        [Test]
        public void Exists_UnexistingFile_False()
        {
            var provider = new ZipStreamProvider(zipFile);
            var result = provider.Exists("NonExisting.xml");
            Assert.IsFalse(result);
        }

        [Test]
        public void GetBytes_ExistingFile_CorrectLength()
        {
            var provider = new ZipStreamProvider(zipFile);
            var bytes = provider.GetBytes("EmployeeSalesDetail.rsd");
            Assert.That(bytes.Length, Is.EqualTo(3697));
        }

        [Test]
        public void GetMemoryStream_ExistingFile_CorrectLength()
        {
            var provider = new ZipStreamProvider(zipFile);
            var memory = provider.GetMemoryStream("EmployeeSalesDetail.rsd");
            Assert.That(memory.Length, Is.EqualTo(3697));
        }

        [Test]
        public void GetMemoryStream_ExistingFile_Position()
        {
            var provider = new ZipStreamProvider(zipFile);
            var memory = provider.GetMemoryStream("EmployeeSalesDetail.rsd");
            Assert.That(memory.Position, Is.EqualTo(0));
        }

        [Test]
        public void GetMemoryStream_ExistingFile_Read()
        {
            var provider = new ZipStreamProvider(zipFile);
            var memory = provider.GetMemoryStream("EmployeeSalesDetail.rsd");
            Assert.That(memory.ReadByte, Is.EqualTo(239));
        }

    }
}
