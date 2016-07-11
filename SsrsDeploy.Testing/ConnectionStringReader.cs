using System;
using System.Xml;
using System.IO;

namespace SsrsDeploy.Testing
{
    class ConnectionStringReader
    {
        public static string Get(string name)
        {
            var xmldoc = new XmlDocument();
            xmldoc.Load(GetFilename());
            XmlNodeList nodes = xmldoc.GetElementsByTagName("add");
            foreach (XmlNode node in nodes)
                if (node.Attributes["name"].Value == name)
                    return node.Attributes["connectionString"].Value;
            throw new Exception();
        }


        private static string GetFilename()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var path = Path.GetDirectoryName(assembly.Location);

            //If available use the user file
            var fullPath = Path.Combine(path, "ConnectionString.user.config");
            if (File.Exists(fullPath))
                return fullPath;

            fullPath = Path.Combine(path, "ConnectionString.config");
            if (File.Exists(fullPath))
                return fullPath;

            throw new InvalidProgramException("No configuration file found");
        }

        internal static string GetReportingServiceUrl()
        {
            return Get("ReportingServiceUrl");
        }

    }
}

