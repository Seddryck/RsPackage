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
    public class ZipStreamProvider : IStreamProvider
    {
        private ZipArchive archive;
        public event EventHandler<MessageEventArgs> MessageSent;

        public ZipStreamProvider()
            : base()
        {

        }

        public Stream GetSolutionStream(string fileName)
        {
            using (archive = ZipFile.Open(fileName, ZipArchiveMode.Read))
            {
                var entry = archive.GetEntry("@[Project].manifest");
                return entry.Open();
            }
        }

        public byte[] GetBytes(string path)
        {
            var entry = archive.GetEntry(path);
            if (entry==null)
            {
                OnError($"File '{path}' doesn't exist in the RsPac!");
                return null;
            }

            Byte[] definition = null;
            try
            {
                using (var stream = entry.Open())
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

        protected virtual void OnInformation(string message)
        {
            var e = MessageEventArgs.Information(message);
            MessageSent?.Invoke(this, e);
        }

        protected virtual void OnWarning(string message)
        {
            var e = MessageEventArgs.Warning(message);
            MessageSent?.Invoke(this, e);
        }

        protected virtual void OnError(string message)
        {
            var e = MessageEventArgs.Error(message);
            MessageSent?.Invoke(this, e);
            throw new InvalidOperationException();
        }

    }
}
