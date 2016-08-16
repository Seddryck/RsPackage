using NUnit.Framework;
using RsPackage.Parser.NamingConventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RsPackage.Testing.Parser.NamingConventions
{
    [TestFixture]
    public class TitleToCamelCaseTest
    {
        [Test]
        public void Apply_ShortNameWithoutSpace_CamelCase()
        {
            var name = "Report";

            var nc = new TitleToCamelCase();
            var result = nc.Apply(name);
            Assert.That(result, Is.EqualTo("Report"));
        }

        [Test]
        public void Apply_LongNameWithSpaces_CamelCase()
        {
            var name = "My test is green";

            var nc = new TitleToCamelCase();
            var result = nc.Apply(name);
            Assert.That(result, Is.EqualTo("MyTestIsGreen"));
        }

        [Test]
        public void Apply_WithOneLetterWord_CamelCase()
        {
            var name = "I am happy";

            var nc = new TitleToCamelCase();
            var result = nc.Apply(name);
            Assert.That(result, Is.EqualTo("IAmHappy"));
        }
    }
}
