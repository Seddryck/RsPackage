using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using SsrsDeploy.ReportingService;

namespace SsrsDeploy.Execution
{
    public class RoleService : BaseService
    {
        public RoleService(ReportingService2010 reportingService) 
            : base(reportingService)
        {}

        public void Create(string name, string description, string[] tasks)
        {
            OnInformation($"Getting tasks' reference for role '{name}'");
            var taskIDs = new List<string>();
            var existingTasks = reportingService.ListTasks("All");
            foreach (var task in tasks.Distinct())
            {
                var existingTask = existingTasks.SingleOrDefault(t => t.Name == task);
                if (existingTask == null)
                    OnWarning($"Task '{task}' not found");
                else
                    taskIDs.Add(existingTask.TaskID);
            }

            if (reportingService.ListRoles("All", null).ToList().Exists(r => r.Name==name))
            {
                OnInformation($"Role '{name}' already existing");
                string preDescription = string.Empty;
                var preTaskIDs = reportingService.GetRoleProperties(name, null, out preDescription);
                var removalTaskIDs = taskIDs.Except(preTaskIDs);
                if (taskIDs.Except(preTaskIDs).Count()==0 && preTaskIDs.Except(taskIDs).Count() == 0 && description==preDescription)
                    OnInformation($"Tasks and description for existing role '{name}' were correctly defined");
                else
                {
                    OnInformation($"Assigning correct tasks and description to existing role '{name}'");
                    reportingService.SetRoleProperties(name, description, taskIDs.ToArray());
                }
            }
                
            else
            {
                OnInformation($"Creating role '{name}'");
                reportingService.CreateRole(name, description, taskIDs.ToArray());
            }
            
        }
    }
}
