using CommandLine;
using RsDeploy.Parser.NamingConventions;
using RsDeploy.Parser.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RsDeploy
{
    class Program
    {
        static int Main(string[] args)
        {
            var result = CommandLine.Parser.Default.ParseArguments<Options>(args);
            var exitCode = result.MapResult(
                o => { Console.WriteLine($"deploying to {o.Url} based on {o.Source}."); return 0; },
                e => { return 1; }
                );

            var argsValue = ((Parsed<Options>)result).Value;

            var rs = new ReportingService.ReportingService2010();
            rs.Url = argsValue.Url;
            rs.Credentials = System.Net.CredentialCache.DefaultCredentials;

            var rootPath = argsValue.Root ?? Path.GetDirectoryName(argsValue.Source);
            rootPath = rootPath.EndsWith(Path.DirectorySeparatorChar.ToString()) ? rootPath : rootPath + Path.DirectorySeparatorChar;

            var parentFolder = argsValue.ParentFolder;

            var parser = new ProjectParser(rs, parentFolder, rootPath, new TitleToUnderscoreWord());

            using(var stream = File.OpenRead(argsValue.Source))
                parser.Execute(stream);

            return exitCode;
        }
    }
}
