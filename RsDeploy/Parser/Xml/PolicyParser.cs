﻿using RsDeploy.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RsDeploy.Parser.Xml
{
    class PolicyParser : IParser
    {
        private PolicyService policyService;
        private IEnumerable<IParser> ChildrenParsers;

        public PolicyParser()
        {
            ChildrenParsers = new List<IParser>();
        }

        public void Execute(XmlNode node, string parent)
        {
            var membershipNodes = node.SelectNodes("Membership");
            if (membershipNodes.Count == 0)
                return;

            var securities = new List<Tuple<string, string[]>>();
            foreach (XmlNode membershipNode in membershipNodes)
            {
                var role = membershipNode.Attributes["Role"].Value;
                var members = new List<string>();
                foreach (XmlNode memberNode in membershipNode.SelectNodes("Task"))
                    members.Add(memberNode.Value);
                var security = new Tuple<string, string[]>(role, members.ToArray());
                securities.Add(security);
            }

            policyService.Create(parent, securities);
        }
    }
}