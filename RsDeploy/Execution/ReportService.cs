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
        public ReportService()
        {}

        public ReportService(ReportingService2010 reportingService) 
            : base(reportingService)
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
            OnInformation($"Creating report '{name}' in '{parent}'");
            reportingService.CreateCatalogItem("Report", name, parent, true, definition, null, out warnings);

            foreach (var warning in warnings)
                OnWarning(warning.Message);
        }

        public virtual void Create(string name, string parent, string path, Dictionary<string, string> dataSources)
        {
            Create(name, parent, path);

            var reportDataSources = reportingService.GetItemDataSources(path);
            OnInformation($"Referencing {reportDataSources.Count()} data sources for report '{name}' in '{parent}'");
            
            foreach (var reportDataSource in reportDataSources)
            {
                if (dataSources.ContainsKey(reportDataSource.Name))
                {
                    var dsRef = new DataSourceReference();
                    var reference = string.Empty;
                    dataSources.TryGetValue("reportDataSource.Name", out reference);
                    dsRef.Reference = reference;

                    reportDataSource.Item = dsRef;
                }
                else
                    OnWarning($"The data source '{reportDataSource.Name}' of the report '{name}' has not been overridden because this data source is not defined in the deployment manifest.");
            }

            reportingService.SetItemDataSources(path, reportDataSources);
        }
    }
}
