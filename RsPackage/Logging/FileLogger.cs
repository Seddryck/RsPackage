using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RsPackage.Logging
{
    class FileLogger : ILogger
    {
        private TextWriter writer;
        private readonly string path;

        public FileLogger(string path)
        {
            this.path = path;
        }

        public void WriteMessage(object sender, MessageEventArgs eventArgs)
        {
            if (writer == null)
                BuildWriter();

            writer.Write(eventArgs.Time.ToString("dd-MM-yyyy hh:mm:ss.fff"));
            writer.Write('\t');
            writer.Write(eventArgs.Level);
            writer.Write('\t');
            writer.WriteLine(eventArgs.Message);
            writer.Flush();
        }

        private void BuildWriter()
        {
            writer = new StreamWriter(this.path);
        }

        public void Dispose()
        {
            writer.Close();
            writer.Dispose();
        }

        
    }
}
