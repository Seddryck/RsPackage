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

namespace RsPackage.StreamProvider
{
    public class ZipStreamProvider : IStreamProvider
    {
        private string archivePath;
        public event EventHandler<MessageEventArgs> MessageSent;

        public ZipStreamProvider(string fileName)
            : base()
        {
            archivePath = fileName;
        }

        public MemoryStream GetMemoryStream(string fileName)
        {
            using (var archive = ZipFile.Open(archivePath, ZipArchiveMode.Read))
            {
                var entry = archive.GetEntry(fileName);
                var readerStream = new MemoryStream();
                entry.Open().CopyTo(readerStream);
                readerStream.Position = 0;
                return readerStream;
            }
        }

        public byte[] GetBytes(string path)
        {
            using (var archive = ZipFile.Open(archivePath, ZipArchiveMode.Read))
            {
                var entry = archive.GetEntry(path);
                if (entry == null)
                {
                    OnError($"File '{path}' doesn't exist in the RsPac!");
                    return null;
                }

                Byte[] definition = null;
                try
                {
                    using (var stream = entry.Open())
                    {
                        definition = new Byte[entry.Length];
                        stream.Read(definition, 0, (int)entry.Length);
                    }
                }
                catch (IOException e)
                {
                    OnInformation(e.Message);
                }

                return definition;
            }
        }

        public bool Exists(string path)
        {
            using (var archive = ZipFile.Open(archivePath, ZipArchiveMode.Read))
            {
                var entry = archive.GetEntry(path);
                return (entry != null);
            }
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
