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
    public class FileStreamProviderTest
    {

        private string fileDirectory;

        [SetUp]
        public void Setup()
        {
            var fullPath = FileOnDisk.CreatePhysicalFile("EmployeeSalesDetail.rsd", "RsPackage.Testing.Resources.EmployeeSalesDetail.rsd");
            fileDirectory = System.IO.Path.GetDirectoryName(fullPath);
        }

        [Test]
        public void Exists_ExistingFile_True()
        {
            var provider = new FileStreamProvider(fileDirectory);
            var result = provider.Exists("EmployeeSalesDetail.rsd");
            Assert.IsTrue(result);
        }

        [Test]
        public void Exists_UnexistingFile_False()
        {
            var provider = new FileStreamProvider(fileDirectory);
            var result = provider.Exists("NonExisting.xml");
            Assert.IsFalse(result);
        }

        [Test]
        public void GetBytes_ExistingFile_CorrectLength()
        {
            var provider = new FileStreamProvider(fileDirectory);
            var bytes = provider.GetBytes("EmployeeSalesDetail.rsd");
            //Due to changes at github for CrLf, difficult to give exact length
            Assert.That(bytes.Length, Is.GreaterThanOrEqualTo(3600));
            Assert.That(bytes.Length, Is.LessThanOrEqualTo(3700));
        }

        [Test]
        public void GetMemoryStream_ExistingFile_CorrectLength()
        {
            var provider = new FileStreamProvider(fileDirectory);
            var memory = provider.GetMemoryStream("EmployeeSalesDetail.rsd");
            //Due to changes at github for CrLf, difficult to give exact length
            Assert.That(memory.Length, Is.GreaterThanOrEqualTo(3600));
            Assert.That(memory.Length, Is.LessThanOrEqualTo(3700));
        }

        [Test]
        public void GetMemoryStream_ExistingFile_Position()
        {
            var provider = new FileStreamProvider(fileDirectory);
            var memory = provider.GetMemoryStream("EmployeeSalesDetail.rsd");
            Assert.That(memory.Position, Is.EqualTo(0));
        }

        [Test]
        public void GetMemoryStream_ExistingFile_Read()
        {
            var provider = new FileStreamProvider(fileDirectory);
            var memory = provider.GetMemoryStream("EmployeeSalesDetail.rsd");
            Assert.That(memory.ReadByte, Is.EqualTo(239));
        }

    }
}
