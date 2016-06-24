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
            Create(name, parent, path, string.Empty, false);
        }

        public virtual void Create(string name, string parent, string path, string description, bool hidden)
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

            var properties = new List<Property>();
            properties.Add(new Property() { Name = "Description", Value = description });
            properties.Add(new Property() { Name = "Hidden", Value = hidden.ToString() });


            reportingService.CreateCatalogItem("Report", name, parent, true, definition, properties.ToArray(), out warnings);

            foreach (var warning in warnings)
                OnWarning(warning.Message);
        }

        public virtual void Create(string name, string parent, string path, string description, bool hidden, Dictionary<string, string> dataSources)
        {
            Create(name, parent, path, description, hidden);

            var reportDataSources = reportingService.GetItemDataSources($"{parent}/{name}");
            OnInformation($"Referencing {reportDataSources.Count()} data sources for report '{name}' in '{parent}'");
            
            foreach (var reportDataSource in reportDataSources)
            {
                if (dataSources.ContainsKey(reportDataSource.Name))
                {
                    var dsRef = new DataSourceReference();
                    dsRef.Reference = dataSources[reportDataSource.Name];
                    reportDataSource.Item = dsRef;
                }
                else
                    OnWarning($"The data source '{reportDataSource.Name}' of the report '{name}' has not been overridden because this data source is not defined in the deployment manifest.");
            }

            if (reportDataSources!=null && !reportDataSources.Any(ds => ds.Item is InvalidDataSourceReference))
                reportingService.SetItemDataSources($"{parent}/{name}", reportDataSources);
            else
                OnError($"The data sources of report '{name}' have not been redirected because some of them are not available in the manifest.");
        }
    }
}
