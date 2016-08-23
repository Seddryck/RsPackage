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
        Stream GetSolutionStream(string fileName);
        event EventHandler<MessageEventArgs> MessageSent;
    }
}
