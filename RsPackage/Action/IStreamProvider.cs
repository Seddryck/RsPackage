using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RsPackage.Action
{
    public interface IStreamProvider
    {
        Byte[] GetBytes(string fileName);
        //Usage of a memory stream to avoid issues such as described at http://stackoverflow.com/questions/30162291/xmldocument-fails-to-load-stream-from-ziparchive-entry
        MemoryStream GetMemoryStream(string fileName);
        event EventHandler<MessageEventArgs> MessageSent;
    }
}
