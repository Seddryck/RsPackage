using CommandLine;
using RsPackage.Factory;
using RsPackage.Parser.Xml;
using RsPackage.Parser.NamingConventions;
using RsPackage.CommandLineArgs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RsPackage.Action;

namespace RsPackage
{
    public class Program
    {
        static int Main(string[] args)
        {
            var result = CommandLine.Parser.Default.ParseArguments<PublishOptions, PackageOptions>(args);
            var exitCode = result.MapResult(
                (PublishOptions o) => Publish(o),
                (PackageOptions o) => Package(o),
                e => 1
                );

            return exitCode;
        }

        protected static int Publish(PublishOptions options)
        {
            Console.WriteLine($"Publishing to {options.Url} based on {options.SourceFile}.");
            if (!string.IsNullOrEmpty(options.LogPath))
                { Console.WriteLine($"Redirecting logs to {options.LogPath}"); }

            var factory = new PublisherFactory();
            var publisher = factory.GetPublisher(options);

            publisher.Execute();

            return 0;
        }

        protected static int Package(PackageOptions options)
        {
            if (!string.IsNullOrEmpty(options.LogPath))
            { Console.WriteLine($"Redirecting logs to {options.LogPath}"); }

            var factory = new PackagerFactory();
            var packager = factory.GetPackager(options);

            packager.Execute();

            return 0;
        }
    }
}
