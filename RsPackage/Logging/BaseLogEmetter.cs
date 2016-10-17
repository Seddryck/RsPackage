using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RsPackage.Logging
{
    public class BaseLogEmetter
    {
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
