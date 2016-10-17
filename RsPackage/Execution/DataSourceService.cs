using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Xml;

using RsPackage.ReportingService;
using RsPackage.Transform;
using RsPackage.StreamProvider;


namespace RsPackage.Execution
{
    public class DataSourceService : CatalogItemService
    {
        public DataSourceService()
            : base("Data source", "datasource")
        { }

        public DataSourceService(ReportingService2010 reportingService, RsPackage.StreamProvider.IStreamProvider streamProvider)
            : base(reportingService, streamProvider, "Data source", "datasource")
        { }

        public virtual void Create(string name, string parent, string path)
        {
            //If file extension is not specied we need to check the existence of both
            if (Path.GetExtension(path)!=".rsds" && Path.GetExtension(path) != ".rds")
            {
                if (StreamProvider.Exists($"{path}.rsds"))
                    path = $"{path}.rsds";
                else
                    path = $"{path}.rds";
            }

            //If file is not found then we must throw an error
            if (!StreamProvider.Exists(path))
            {
                OnError($"File '{path}' doesn't exist!");
                return;
            }

            // If file is a Visual Studio connection-string then we need to transform it
            XmlDocument dataSourceDocument = null;
            if (Path.GetExtension(path) == ".rds")
            {
                var document = new XmlDocument();
                document.Load(StreamProvider.GetMemoryStream(path));

                var transformer = new DataSourceTransformer();
                if (transformer.IsVisualStudio(document))
                    dataSourceDocument = transformer.ToReportingService(document);
                else
                    throw new InvalidOperationException();
            }

            //If it's a rsds file we need to ensure it's a valid Reporting Server connection string
            else if (Path.GetExtension(path) == ".rsds")
            {
                dataSourceDocument.Load(StreamProvider.GetMemoryStream(path));

                var transformer = new DataSourceTransformer();
                if (!transformer.IsReportingService(dataSourceDocument))
                    throw new InvalidOperationException();
            }

            Byte[] definition = null;
            try
            {
                using (var stream = new MemoryStream())
                {
                    dataSourceDocument.Save(stream);
                    stream.Flush();
                    stream.Position = 0;
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
