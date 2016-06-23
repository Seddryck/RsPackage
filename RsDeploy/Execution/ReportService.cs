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
    }
}
