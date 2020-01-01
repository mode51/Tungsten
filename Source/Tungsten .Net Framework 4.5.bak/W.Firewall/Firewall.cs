#if NET20 || NET45
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
        /// Firewall protocols
        /// </summary>
        public enum EFirewallProtocol
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
            /// Allow communications
            /// </summary>
            Allowed = NET_FW_ACTION_.NET_FW_ACTION_ALLOW,
            /// <summary>
            /// Block communications
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
        /// Firewall rule direction
        /// </summary>
        public enum EFirewallRuleDirection
        {
            /// <summary>
            /// Inbound rule
            /// </summary>
            In = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN,
            /// <summary>
            /// Outbound rule
            /// </summary>
            Out = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT,
            /// <summary>
            /// Max
            /// </summary>
            Max = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_MAX,
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
        /// <returns>True if the rule was created, False if it already exists</returns>
        public static bool Add(string ruleName, string ruleGroup, EFirewallProtocol protocol = EFirewallProtocol.Tcp, string localPorts = "80", EFirewallRuleAction action = EFirewallRuleAction.Allowed, EFirewallProfiles profiles = EFirewallProfiles.All)
        {
            if (Exists(ruleName))
                return false;
            Type tNetFwPolicy2 = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            INetFwPolicy2 fwPolicy2 = (INetFwPolicy2)Activator.CreateInstance(tNetFwPolicy2);
            var currentProfiles = fwPolicy2.CurrentProfileTypes;

            // Let's create a new rule
            INetFwRule2 inboundRule = (INetFwRule2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
            inboundRule.Enabled = true;

            //firewall rule
            inboundRule.Action = (NET_FW_ACTION_)action;
            //specify protocol
            inboundRule.Protocol = (int) protocol; 
            //specify ports
            inboundRule.LocalPorts = localPorts;
            //Name of rule
            inboundRule.Name = ruleName;
            // ...//
            inboundRule.Grouping = ruleGroup;
            inboundRule.Profiles = (int)profiles;

            // Now add the rule
            fwPolicy2.Rules.Add(inboundRule);
            //INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
            //firewallPolicy.Rules.Add(inboundRule);
            return true;
        }
        /// <summary>
        /// Create a firewall rule
        /// </summary>
        /// <param name="name">The name of the firewall rule</param>
        /// <param name="description">The description</param>
        /// <param name="applicationName">The application name</param>
        /// <param name="serviceName">The service name</param>
        /// <param name="protocol">The  protocol</param>
        /// <param name="localPorts">The local port(s)</param>
        /// <param name="remotePorts">The remote port(s)</param>
        /// <param name="localAddresses">The local address(es)</param>
        /// <param name="remoteAddresses">The remote address(es)</param>
        /// <param name="IcmpTypesAndCodes">IcmpTypesAndCodes</param>
        /// <param name="direction">The rule direction</param>
        /// <param name="interfaces">interfaces</param>
        /// <param name="interfaceTypes">interfaceTypes</param>
        /// <param name="enabled">Whether the rule is enabled or not</param>
        /// <param name="grouping">grouping</param>
        /// <param name="profiles">profiles</param>
        /// <param name="edgeTraversal">edgeTraversal</param>
        /// <param name="action">action</param>
        /// <param name="edgeTraversalOptions">edgeTraversalOptions</param>
        /// <returns>True if the rule was created, False if it already exists</returns>
        /// <remarks>I have not tested all scenarios.  Please reports any issues.</remarks>
        public static bool Add(string name, string description, string applicationName, string serviceName, int protocol, string localPorts, string remotePorts, string localAddresses, string remoteAddresses, string IcmpTypesAndCodes, EFirewallRuleDirection direction, object interfaces, string interfaceTypes, bool enabled, string grouping, int profiles, bool edgeTraversal, EFirewallRuleAction action, int edgeTraversalOptions)
        {
            if (Exists(name))
                return false;
            Type tNetFwPolicy2 = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            INetFwPolicy2 fwPolicy2 = (INetFwPolicy2)Activator.CreateInstance(tNetFwPolicy2);
            var currentProfiles = fwPolicy2.CurrentProfileTypes;

            // Let's create a new rule
            INetFwRule2 rule = (INetFwRule2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));

            rule.Name = name;
            if (!string.IsNullOrEmpty(description))
                rule.Description = description;
            if (!string.IsNullOrEmpty(applicationName))
                rule.ApplicationName = applicationName;
            if (!string.IsNullOrEmpty(serviceName))
                rule.serviceName = serviceName;
            rule.Protocol = (int)protocol;
            if (!string.IsNullOrEmpty(localPorts))
                rule.LocalPorts = localPorts;
            if (!string.IsNullOrEmpty(remotePorts))
                rule.RemotePorts = remotePorts;
            if (!string.IsNullOrEmpty(localAddresses))
                rule.LocalAddresses = localAddresses;
            if (!string.IsNullOrEmpty(remoteAddresses))
                rule.RemoteAddresses = remoteAddresses;
            if (!string.IsNullOrEmpty(IcmpTypesAndCodes))
                rule.IcmpTypesAndCodes = IcmpTypesAndCodes;
            rule.Direction = (NET_FW_RULE_DIRECTION_)direction;
            if (interfaces != null)
                rule.Interfaces = interfaces;
            if (!string.IsNullOrEmpty(interfaceTypes))
                rule.InterfaceTypes = interfaceTypes;
            rule.Enabled = enabled;
            if (!string.IsNullOrEmpty(grouping))
                rule.Grouping = grouping;
            rule.Profiles = (int)profiles;
            rule.EdgeTraversal = edgeTraversal;
            rule.Action = (NET_FW_ACTION_)action;
            rule.EdgeTraversalOptions = edgeTraversalOptions;

            // Now add the rule
            fwPolicy2.Rules.Add(rule);
            return true;
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
        /// <returns>True if the rule exists and was removed, otherwise False</returns>
        public static bool Remove(string ruleName)
        {
            if (Exists(ruleName))
            {
                INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                firewallPolicy.Rules.Remove(ruleName);
                return true;
            }
            return false;
        } 
    }
}
#endif