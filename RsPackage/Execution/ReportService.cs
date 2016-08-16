using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using RsPackage.ReportingService;

namespace RsPackage.Execution
{
    public class ReportService : BaseService
    {
        public ReportService()
        {}

        public ReportService(ReportingService2010 reportingService) 
            : base(reportingService)
        {}

        public virtual Warning[] Create(string name, string parent, string path)
        {
            return Create(name, parent, path, string.Empty, false);
        }

        public virtual Warning[] Create(string name, string parent, string path, string description, bool hidden)
        {
            if (!File.Exists(path))
            {
                OnError($"File '{path}' doesn't exist!");
                return null;
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

            try
            {
                reportingService.CreateCatalogItem("Report", name, parent, true, definition, properties.ToArray(), out warnings);
            }
            catch (Exception ex)
            {

                OnError(ex.Message);
            }
            

            return warnings;
        }

        public virtual void Create(string name, string parent, string path, string description, bool hidden, IDictionary<string, string> dataSources)
        {
            var warnings = Create(name, parent, path, description, hidden);
            if (warnings != null)
            {
                foreach (var warning in warnings)
                {
                    if (!(warning.Code == "rsDataSourceReferenceNotPublished"  && dataSources.ContainsKey(warning.ObjectName)))
                        OnWarning(warning.Message);
                }
            }
                

            var reportDataSources = reportingService.GetItemDataSources($"{parent}/{name}");
            if (reportDataSources.Count()>1)
                OnInformation($"Trying to reference {reportDataSources.Count()} data sources for report '{name}' in '{parent}'");
            
            foreach (var reportDataSource in reportDataSources)
            {
                if (dataSources!=null && dataSources.ContainsKey(reportDataSource.Name))
                {
                    var dsRef = new DataSourceReference();
                    dsRef.Reference = dataSources[reportDataSource.Name];
                    reportDataSource.Item = dsRef;
                    OnInformation($"Reference for data source '{reportDataSource.Name}' of report '{name}' found and re-assigned.");
                }
                else
                    OnWarning($"The data source '{reportDataSource.Name}' of the report '{name}' has not been re-assigned because this data source is not defined in the deployment manifest.");
            }

            if (reportDataSources!=null && !reportDataSources.Any(ds => ds.Item is InvalidDataSourceReference))
                reportingService.SetItemDataSources($"{parent}/{name}", reportDataSources);
            else
                OnError($"The data sources of report '{name}' have not been redirected because some of them are not available in the manifest.");
        }
    }
}
