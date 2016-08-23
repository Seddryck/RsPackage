using RsPackage.Action;
using RsPackage.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RsPackage.Parser.Xml
{
    public class PolicyParser : IParser
    {
        private PolicyService policyService;
        private IEnumerable<IParser> ChildrenParsers;

        public Publisher Root { get; set; }
        public IParser Parent { get; set; }
        public string ParentPath { get; set; }

        public PolicyParser()
        { }

        public PolicyParser(PolicyService policyService)
        {
            this.policyService = policyService;
            ChildrenParsers = new List<IParser>();
        }

        public virtual void Execute(XmlNode node)
        {
            var membershipNodes = node.SelectNodes("Membership");
            if (membershipNodes.Count == 0)
                return;

            var securities = new List<Tuple<string, string[]>>();
            foreach (XmlNode membershipNode in membershipNodes)
            {
                var role = membershipNode.Attributes["Role"].Value;
                var members = new List<string>();
                foreach (XmlNode memberNode in membershipNode.SelectNodes("Member"))
                    members.Add(memberNode.InnerText);
                var security = new Tuple<string, string[]>(role, members.ToArray());
                securities.Add(security);
            }

            policyService.Create(ParentPath, securities);
        }
    }
}
