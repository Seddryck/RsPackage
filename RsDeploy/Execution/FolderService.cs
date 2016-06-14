using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using RsDeploy.ReportingService;

namespace RsDeploy.Execution
{
    public class FolderService : BaseService
    {
        public FolderService(ReportingService2010 reportingService) 
            : base(reportingService)
        {}

        public void Create(string folder, string parent)
        {
            if (reportingService.GetItemType($"{parent}{folder}") == "Folder")
                OnInformation($"Folder '{folder}' already existing");
            else
            {
                OnInformation($"Creating folder '{folder}'");
                reportingService.CreateFolder(folder, parent, null);
            }
            
        }
    }
}
