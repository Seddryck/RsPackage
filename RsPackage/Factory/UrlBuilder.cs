using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RsPackage.Factory
{
    public class UrlBuilder
    {
        private Options options;
        private UriBuilder builder;

        public void Setup(Options options)
        {
            this.options = options;
        }

        public void Build()
        {
            var baseUrl = options.Url;
            builder = new UriBuilder(baseUrl);

            if (string.IsNullOrEmpty(builder.Scheme))
                builder.Scheme = Uri.UriSchemeHttp;

            if (builder.Scheme != Uri.UriSchemeHttp && builder.Scheme != Uri.UriSchemeHttps)
                throw new ArgumentException();

            if (!builder.Path.EndsWith("/ReportService2010.asmx"))
                builder.Path += (builder.Path.EndsWith("/") ? "" : "/") + "ReportService2010.asmx";

            if (builder.Path.Equals("/ReportService2010.asmx"))
                builder.Path = "/ReportServer" + builder.Path;
        }

        public string GetUrl()
        {
            if (builder.Uri.IsDefaultPort)
                return builder.Uri.GetComponents(UriComponents.AbsoluteUri & ~UriComponents.Port, UriFormat.UriEscaped);

            return builder.ToString();
        }
    }
}
