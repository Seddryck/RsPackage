using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using RsPackage.ReportingService;
using RsPackage.StreamProvider;

namespace RsPackage.Execution
{
    public class SharedDatasetService : CatalogItemService
    {
        public SharedDatasetService()
            : base("DataSet", "data set")
        { }

        public SharedDatasetService(ReportingService2010 reportingService, IStreamProvider streamProvider)
            : base(reportingService, streamProvider, "DataSet", "data set")
        { }

        public virtual void Create(string name, string parent, string path, string description, bool hidden, string dataSourceName, IDictionary<string, string> dataSources)
        {
            var warnings = Create(name, parent, path, description, hidden);
            if (warnings != null)
            {
                foreach (var warning in warnings)
                {
                    if (!(warning.Code == "rsDataSourceReferenceNotPublished" && dataSources.ContainsKey(dataSourceName)))
                        OnWarning(warning.Message);
                }
            }
            ReferenceItem(name, parent, dataSourceName, dataSources);
        }

        protected virtual void ReferenceItem(string name, string parent, string reference, IDictionary<string, string> references)
        {
            var item = reportingService.GetItemReferences($"{parent}/{name}", "DataSource")[0];

            var dataSourceRef = new ItemReference();
            if (references != null && references.ContainsKey(reference))
            {
                dataSourceRef.Reference = references[reference];
                dataSourceRef.Name = item.Name;
                reportingService.SetItemReferences($"{parent}/{name}", Enumerable.Repeat(dataSourceRef, 1).ToArray());
                OnInformation($"Reference to data source '{reference}' in data set '{name}' found and re-assigned.");
            }
            else
                OnError($"The data source '{reference}' of the data set '{name}' has not been re-assigned because this data source is not defined in the deployment manifest.");
        }
    }
}
