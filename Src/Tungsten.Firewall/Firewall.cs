﻿#if NET20 || NET45
using System;
using System.Collections.Generic;
using System.Text;
using NetFwTypeLib;

namespace W.Firewall
{
    /// <summary>
    /// Provides static methods to add, remove and check the existance of, Windows firewall rules
    /// </summary>
    public static class Rules
    {
        /// <summary>
        /// Firewall Protocols
        /// </summary>
        public enum EFirewallProtocols
        {
            /// <summary>
            /// Any
            /// </summary>
            Any = NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_ANY,
            /// <summary>
            /// Tcp
            /// </summary>
            Tcp = NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP,
            /// <summary>
            /// Udp
            /// </summary>
            Udp = NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_UDP
        }
        /// <summary>
        /// Firewall rule actions
        /// </summary>
        public enum EFirewallRuleAction
        {
            /// <summary>
            /// Allow
            /// </summary>
            Allowed = NET_FW_ACTION_.NET_FW_ACTION_ALLOW,
            /// <summary>
            /// Block
            /// </summary>
            Block = NET_FW_ACTION_.NET_FW_ACTION_BLOCK,
            /// <summary>
            /// Max
            /// </summary>
            Max = NET_FW_ACTION_.NET_FW_ACTION_MAX
        }
        /// <summary>
        /// The firewall profile type
        /// </summary>
        public enum EFirewallProfiles
        {
            /// <summary>
            /// Public
            /// </summary>
            Public = NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_PUBLIC,
            /// <summary>
            /// Private
            /// </summary>
            Private = NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_PRIVATE,
            /// <summary>
            /// Domain
            /// </summary>
            Domain = NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_DOMAIN,
            /// <summary>
            /// All
            /// </summary>
            All = NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_ALL
        }

        /// <summary>
        /// Adds a rule to the firewall
        /// </summary>
        /// <param name="ruleName">The name of the rule to add</param>
        /// <param name="ruleGroup">The group under which the rule is added</param>
        /// <param name="protocol">The desired rule protocol</param>
        /// <param name="localPorts">The desired rule port</param>
        /// <param name="action">The desired rule action, to allow or block communications</param>
        /// <param name="profiles">The desired rule profile</param>
        public static void Add(string ruleName, string ruleGroup, EFirewallProtocols protocol = EFirewallProtocols.Tcp, string localPorts = "80", EFirewallRuleAction action = EFirewallRuleAction.Allowed, EFirewallProfiles profiles = EFirewallProfiles.All)
        {
            if (Exists(ruleName))
                return;
            Type tNetFwPolicy2 = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            INetFwPolicy2 fwPolicy2 = (INetFwPolicy2)Activator.CreateInstance(tNetFwPolicy2);
            var currentProfiles = fwPolicy2.CurrentProfileTypes;

            // Let's create a new rule
            INetFwRule2 inboundRule = (INetFwRule2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
            inboundRule.Enabled = true;

            //firewall rule
            inboundRule.Action = (NET_FW_ACTION_)action;
            //specify protocol
            inboundRule.Protocol = (int)protocol;
            //specify ports
            inboundRule.LocalPorts = localPorts;
            //Name of rule
            inboundRule.Name = ruleName;
            // ...//
            inboundRule.Grouping = ruleGroup;
            inboundRule.Profiles = (int)profiles;

            // Now add the rule
            INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
            firewallPolicy.Rules.Add(inboundRule);
        }
        /// <summary>
        /// Checks if a particular rule exists
        /// </summary>
        /// <param name="ruleName">The name of the rule to check</param>
        /// <returns>True if the rule exists, otherwise false</returns>
        public static bool Exists(string ruleName)
        {
            bool result = false;
            INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));

            foreach (INetFwRule rule in firewallPolicy.Rules)
            {
                if (rule.Name == ruleName)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
        /// <summary>
        /// Removes a firewall rule
        /// </summary>
        /// <param name="ruleName">The name of the rule to remove</param>
        public static void Remove(string ruleName)
        {
            if (Exists(ruleName))
            {
                INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                firewallPolicy.Rules.Remove(ruleName);
            }
        }
    }
}
#endif