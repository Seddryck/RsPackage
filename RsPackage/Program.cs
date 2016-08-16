using CommandLine;
using RsPackage.Factory;
using RsPackage.Parser.Xml;
using RsPackage.Parser.NamingConventions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RsPackage
{
    public class Program
    {
        static int Main(string[] args)
        {
            var result = CommandLine.Parser.Default.ParseArguments<Options>(args);
            var exitCode = result.MapResult(
                o => { Console.WriteLine($"deploying to {o.Url} based on {o.SourceFile}."); if (!string.IsNullOrEmpty(o.LogPath)) { Console.WriteLine($"Redirecting logs to {o.LogPath}"); } return 0; },
                e => { return 1; }
                );

            var argsValue = ((Parsed<Options>)result).Value;

            var factory = new ParserFactory();
            var parser = factory.GetXmlParser(argsValue);

            using(var stream = File.OpenRead(argsValue.SourceFile))
                parser.Execute(stream);

            return exitCode;
        }
    }
}
