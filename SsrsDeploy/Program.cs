using CommandLine;
using SsrsDeploy.Parser.NamingConventions;
using SsrsDeploy.Parser.Xml;
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

            var rs = new ReportingService.ReportingService2010();
            rs.Url = GetUrl(argsValue);
            rs.Credentials = System.Net.CredentialCache.DefaultCredentials;

            var rootPath = argsValue.Root ?? Path.GetDirectoryName(argsValue.Source);
            rootPath = rootPath.EndsWith(Path.DirectorySeparatorChar.ToString()) ? rootPath : rootPath + Path.DirectorySeparatorChar;

            var parentFolder = argsValue.ParentFolder;

            var parser = new ProjectParser(rs, parentFolder, rootPath, new TitleToUnderscoreWord());

            using(var stream = File.OpenRead(argsValue.Source))
                parser.Execute(stream);

            return exitCode;
        }

        public static string GetUrl(Options options)
        {
            var baseUrl = options.Url;
            var builder = new UriBuilder(baseUrl);

            if (string.IsNullOrEmpty(builder.Scheme))
                builder.Scheme = Uri.UriSchemeHttp;

            if (builder.Scheme != Uri.UriSchemeHttp && builder.Scheme != Uri.UriSchemeHttps)
                throw new ArgumentException();

            if (!builder.Path.EndsWith("/ReportService2010.asmx"))
                builder.Path += (builder.Path.EndsWith("/") ? "" : "/") + "ReportService2010.asmx";

            if (builder.Path.Equals("/ReportService2010.asmx"))
                builder.Path = "/ReportServer" + builder.Path;

            if (builder.Uri.IsDefaultPort)
                return builder.Uri.GetComponents(UriComponents.AbsoluteUri & ~UriComponents.Port, UriFormat.UriEscaped);

            return builder.ToString();
        }
    }
}
