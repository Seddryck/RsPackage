using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RsDeploy.ReportingService;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace RsDeploy
{
    public class BaseService
    {
        public event EventHandler<MessageEventArgs> MessageSent;

        protected ReportingService2010 reportingService { get; }

        public BaseService()
        { }

        public BaseService(ReportingService2010 reportingService)
        {
            this.reportingService = reportingService;
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
