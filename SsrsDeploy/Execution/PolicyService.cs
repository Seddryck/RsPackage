using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using SsrsDeploy.ReportingService;

namespace SsrsDeploy.Execution
{
    public class PolicyService : BaseService
    {
        public PolicyService()
        { }

        public PolicyService(ReportingService2010 reportingService)
            : base(reportingService)
        { }

        public virtual void Create(string parent, IEnumerable<Tuple<string, string[]>> securities, bool inherit=true)
        {
            var inheritString = inherit ? "inheritance" : "override";
            OnInformation($"Configuring policies by {inheritString} for '{parent}'");

            var existingRoles = reportingService.ListRoles("All", null);

            var roles = securities.Select(p => p.Item1).Distinct();
            if (roles.Except(existingRoles.Select(r => r.Name)).Count() > 0)
                OnError($"Following roles don't exist: {String.Join(", ", roles.Except(existingRoles.Select(r => r.Name)))}");

            var userNames = securities.SelectMany(p => p.Item2).Distinct();

            bool boolParam;
            var grandParent = parent=="/" ? "/" : string.Join("/", parent.Split('/').Reverse().Skip(1).Reverse());
            grandParent = string.IsNullOrEmpty(grandParent) ? "/" : grandParent;
            var inheritedPolicies = inherit ? reportingService.GetPolicies(grandParent, out boolParam) : new Policy[0];

            var policies = new List<Policy>();
            foreach (var userName in userNames)
            {
                var policy = new Policy()
                {
                    GroupUserName = userName,
                    Roles = existingRoles.Where(r => securities.Where(s => s.Item2.Contains(userName)).Select(s => s.Item1).Contains(r.Name)).ToArray()
                };

                policies.Add(policy);
            }

            foreach (var policy in policies)
            {
                var inheritedPolicy = inheritedPolicies.FirstOrDefault(p => p.GroupUserName == policy.GroupUserName);
                if (inheritedPolicy != null)
                    policy.Roles = policy.Roles.ToList().Union(inheritedPolicy.Roles, new RoleEqualityComparer()).Distinct().ToArray();

                var pluralRole = policy.Roles.Count() > 1 ? "s" : string.Empty;
                var rolesString = string.Join(", ", policy.Roles.Select(r => r.Name));
                var verb = inheritedPolicy == null ? "Defining new" : inherit ? "Overloading" : "Overriding";
                OnInformation($"{verb} policy for {policy.GroupUserName} with {policy.Roles.Count()} role{pluralRole}: {rolesString}");
            }

            foreach (var inheritedPolicy in inheritedPolicies)
            {
                var policy = policies.FirstOrDefault(p => p.GroupUserName == inheritedPolicy.GroupUserName);
                if (policy == null)
                {
                    policies.Add(inheritedPolicy);

                    var pluralRole = inheritedPolicy.Roles.Count() > 1 ? "s" : string.Empty;
                    var rolesString = string.Join(", ", inheritedPolicy.Roles.Select(r => r.Name));
                    OnInformation($"Using pre-existing policy for {inheritedPolicy.GroupUserName} with {inheritedPolicy.Roles.Count()} role{pluralRole}: {rolesString}");
                }
            }
            
            var pluralPolicy = policies.Count() > 1 ? "ies" : "y";
            OnInformation($"Assigning {policies.Count} polic{pluralPolicy} for '{parent}'");

            try
            {
                reportingService.SetPolicies(parent, policies.ToArray());
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The user or group name") && ex.Message.Contains("is not recognized."))
                    OnWarning($"{ex.Message.Split(':')[1].Split('.')[0].Trim()}. Policies for the folder/report '{parent}' have been skipped.");
                else
                    throw;
            }

        }
    }
}
