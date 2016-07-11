using NUnit.Framework;
using RsDeploy.Parser.NamingConventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RsDeploy.Testing.Parser.NamingConventions
{
    [TestFixture]
    public class TitleToUnderscoreWordTest
    {
        [Test]
        public void Apply_ShortNameWithoutSpace_UnderscoreCase()
        {
            var name = "Report";

            var nc = new TitleToUnderscoreWord();
            var result = nc.Apply(name);
            Assert.That(result, Is.EqualTo("Report"));
        }

        [Test]
        public void Apply_LongNameWithSpaces_UnderscoreCase()
        {
            var name = "My test is green";

            var nc = new TitleToUnderscoreWord();
            var result = nc.Apply(name);
            Assert.That(result, Is.EqualTo("My_Test_Is_Green"));
        }

        [Test]
        public void Apply_WithOneLetterWord_UnderscoreCase()
        {
            var name = "I am happy";

            var nc = new TitleToUnderscoreWord();
            var result = nc.Apply(name);
            Assert.That(result, Is.EqualTo("I_Am_Happy"));
        }
    }
}
