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

namespace RsPackage.StreamProvider
{
    public class FileStreamProvider : IStreamProvider
    {
        private string rootDirectory;
        public event EventHandler<MessageEventArgs> MessageSent;

        public FileStreamProvider(string rootDirectory)
            : base()
        {
            this.rootDirectory = rootDirectory.EndsWith(@"\") ? rootDirectory : rootDirectory + @"\";
        }

        public virtual MemoryStream GetMemoryStream(string fileName)
        {
            using (var stream = File.OpenRead(rootDirectory + fileName))
            {
                var readerStream = new MemoryStream();
                stream.CopyTo(readerStream);
                readerStream.Position = 0;
                return readerStream;
            }
        }

        public virtual byte[] GetBytes(string path)
        {
            if (!File.Exists(rootDirectory + path))
            {
                OnError($"File '{rootDirectory + path}' doesn't exist!");
                return null;
            }

            Byte[] definition = null;
            try
            {
                using (FileStream stream = File.OpenRead(rootDirectory + path))
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

        public bool Exists(string fileName)
        {
            return File.Exists(rootDirectory + fileName);
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
