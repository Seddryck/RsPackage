using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using RsPackage.ReportingService;

namespace RsPackage.Execution
{
    public class DataSourceService : BaseService
    {
        public DataSourceService()
        {}

        public DataSourceService(ReportingService2010 ReportingService) 
            : base(ReportingService)
        {}

        public virtual void Create(string name, string parent, string path)
        {
            if (!File.Exists(path))
            {
                OnError($"File '{path}' doesn't exist!");
                return;
            }
                

            Byte[] definition = null;
            try
            {
                using (FileStream stream = File.OpenRead(path))
                {
                    definition = new Byte[stream.Length];
                    stream.Read(definition, 0, (int)stream.Length);
                }
            }
            catch (IOException e)
            {
                OnInformation(e.Message);
            }

            Warning[] warnings = null;
            OnInformation($"Creating DataSource '{name}' in '{parent}'");
            reportingService.CreateCatalogItem("DataSource", name, parent, true, definition, null, out warnings);

            if (warnings != null)
                foreach (var warning in warnings)
                    OnWarning(warning.Message);
        }
    }
}
