using RsPackage.Execution;
using RsPackage.Parser;
using RsPackage.Parser.NamingConventions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RsPackage.Action
{
    public class FilesPublisher : AbstractPublisher
    {

        public FilesPublisher()
            : base()
        {
        }

        public override void Execute()
        {
            using (var stream = File.OpenRead(SourceFile))
                this.Execute(stream);
        }

        public override Byte[] GetBytes(string path)
        {
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
