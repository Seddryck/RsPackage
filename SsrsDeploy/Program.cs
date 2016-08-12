using CommandLine;
using SsrsDeploy.Factory;
using SsrsDeploy.Parser.Xml;
using SsrsDeploy.Parser.NamingConventions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SsrsDeploy
{
    public class Program
    {
        static int Main(string[] args)
        {
            var result = CommandLine.Parser.Default.ParseArguments<Options>(args);
            var exitCode = result.MapResult(
                o => { Console.WriteLine($"deploying to {o.Url} based on {o.Source}."); return 0; },
                e => { return 1; }
                );

            var argsValue = ((Parsed<Options>)result).Value;

            var factory = new ParserFactory();
            var parser = factory.GetXmlParser(argsValue);

            using(var stream = File.OpenRead(argsValue.Source))
                parser.Execute(stream);

            return exitCode;
        }
    }
}
