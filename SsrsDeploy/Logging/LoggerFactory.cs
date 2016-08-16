using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SsrsDeploy.Logging
{
    class LoggerFactory
    {
        public ILogger Build(Options options)
        {
            if (string.IsNullOrEmpty(options.LogPath))
                return new ConsoleLogger();
            else
                return new FileLogger(options.LogPath);
        }
    }
}
