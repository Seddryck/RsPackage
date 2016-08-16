using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RsPackage.Logging
{
    class NullLogger : ILogger
    {
        public void Dispose()
        {
            return;
        }

        public void WriteMessage(object sender, MessageEventArgs e)
        {
            return;
        }
    }
}
