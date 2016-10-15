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
    public class FileStreamProvider : IStreamProvider
    {
        public event EventHandler<MessageEventArgs> MessageSent;

        public FileStreamProvider()
            : base()
        {
        }

        public virtual MemoryStream GetMemoryStream(string fileName)
        {
            using (var stream = File.OpenRead(fileName))
            {
                var readerStream = new MemoryStream();
                stream.CopyTo(readerStream);
                readerStream.Position = 0;
                return readerStream;
            }
        }

        public virtual byte[] GetBytes(string path)
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
