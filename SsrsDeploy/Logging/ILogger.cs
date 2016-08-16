using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SsrsDeploy.Logging
{
    public interface ILogger : IDisposable
    {
        void WriteMessage(object sender, MessageEventArgs e);
    }
}
