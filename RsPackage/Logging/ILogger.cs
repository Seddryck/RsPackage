using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RsPackage.Logging
{
    public interface ILogger : IDisposable
    {
        void WriteMessage(object sender, MessageEventArgs e);
    }
}
