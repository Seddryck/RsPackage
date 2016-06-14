using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using RsDeploy.ReportingService;

namespace RsDeploy.Execution
{
    public class ReportService : BaseService
    {
        public ReportService(ReportingService2010 reportingService) 
            : base(reportingService)
        {}

        public void Create(string name, string parent, string path)
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
            OnInformation($"Creating report '{name}' in '{parent}'");
            reportingService.CreateCatalogItem("Report", name, parent, true, definition, null, out warnings);

            foreach (var warning in warnings)
                OnWarning(warning.Message);
        }
    }
}
