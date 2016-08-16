using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using RsPackage.ReportingService;

namespace RsPackage.Execution
{
    public class FolderService : BaseService
    {
        public FolderService()
        { }

        public FolderService(ReportingService2010 reportingService) 
            : base(reportingService)
        {}

        public virtual void Create(string folder, string parent)
        {
            var fullPath = parent == "/" ? $"/{folder}" : $"{parent}/{folder}";
            if (reportingService.GetItemType(fullPath) == "Folder")
                OnInformation($"Folder '{folder}' already existing");
            else
            {
                OnInformation($"Creating folder '{folder}'");
                reportingService.CreateFolder(folder, parent, null);
            }
            
        }
    }
}
