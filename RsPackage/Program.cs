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
            var result = CommandLine.Parser.Default.ParseArguments<PublishOptions>(args);
            var exitCode = result.MapResult(
                o => { return Publish(o); },
                e => { return 1; }
                );

            return exitCode;
        }

        protected static int Publish(PublishOptions options)
        {
            Console.WriteLine($"Publishing to {options.Url} based on {options.SourceFile}.");
            if (!string.IsNullOrEmpty(options.LogPath))
                { Console.WriteLine($"Redirecting logs to {options.LogPath}"); }

            var factory = new ParserFactory();
            var parser = factory.GetXmlParser(options);

            using (var stream = File.OpenRead(options.SourceFile))
                parser.Execute(stream);

            return 0;
        }
    }
}
