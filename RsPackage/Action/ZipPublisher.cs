using RsPackage.Execution;
using RsPackage.Parser;
using RsPackage.Parser.NamingConventions;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RsPackage.Action
{
    public class ZipPublisher : AbstractPublisher
    {
        private ZipArchive archive;

        public ZipPublisher()
            : base()
        {
            
        }

        public override void Execute()
        {
            using (archive = ZipFile.Open(SourceFile, ZipArchiveMode.Read))
            {
                var entry = archive.GetEntry("@[Project].manifest");
                using (var stream = entry.Open())
                    this.Execute(stream);
            }           
        }

        public override Byte[] GetBytes(string path)
        {
            var entry = archive.GetEntry(path);

            //TODO !!!
            if (!File.Exists(path))
            {
                OnError($"File '{path}' doesn't exist!");
                return null;
            }

            Byte[] definition = null;
            try
            {
                using (FileStream stream = File.OpenRead(path))
                {
                    definition = new Byte[stream.Length];
                    stream.Read(definition, 0, (int)stream.Length);
                }
            }
            catch (IOException e)
            {
                OnInformation(e.Message);
            }

            return definition;
        }

    }
}
