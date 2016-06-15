using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using RsDeploy.ReportingService;

namespace RsDeploy.Execution
{
    public class PolicyService : BaseService
    {
        public PolicyService(ReportingService2010 reportingService)
            : base(reportingService)
        { }

        public void Create(string parent, IEnumerable<Tuple<string, string[]>> securities)
        {
            OnInformation($"Configuring policies for '{parent}'");

            var existingRoles = reportingService.ListRoles("All", null);

            var roles = securities.Select(p => p.Item1).Distinct();
            if (roles.Except(existingRoles.Select(r => r.Name)).Count() > 0)
                OnError($"Following roles don't exist: {String.Join(", ", roles.Except(existingRoles.Select(r => r.Name)))}");

            var userNames = securities.SelectMany(p => p.Item2).Distinct();

            var policies = new List<Policy>();
            foreach (var userName in userNames)
            {
                var policy = new Policy()
                {
                    GroupUserName = userName,
                    Roles = existingRoles.Where(r => securities.Where(s => s.Item2.Contains(userName)).Select(s => s.Item1).Contains(r.Name)).ToArray()
                };
                OnInformation($"Defining policy for {userName} with {policy.Roles.Count()} roles");
                policies.Add(policy);
            }
            OnInformation($"Assigning {policies.Count} policies for '{parent}'");

            reportingService.SetPolicies(parent, policies.ToArray());
        }
    }
}
