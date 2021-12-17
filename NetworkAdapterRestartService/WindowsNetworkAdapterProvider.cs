using System;
using System.Management;
using Microsoft.Extensions.Logging;
using NetworkAdapterRestartService.Interfaces;

namespace NetworkAdapterRestartService
{
    internal class WindowsNetworkAdapterProvider : INetworkAdapterProvider
    {
        private readonly ILogger<WindowsNetworkAdapterProvider> _logger;

        public WindowsNetworkAdapterProvider(ILogger<WindowsNetworkAdapterProvider> logger)
        {
            _logger = logger;
        }

        public void ResetAdapter()
        {
            var wmiQuery = new SelectQuery("SELECT * FROM MSFT_NetAdapter WHERE ConnectorPresent=1");
            using var searcher = new ManagementObjectSearcher(new ManagementScope("root\\StandardCimv2"), wmiQuery);
            using var netAdapters = searcher.Get();

            foreach (var netAdapter in netAdapters)
            {
                //The locally unique identifier for the network interface. in InterfaceType_NetluidIndex format. Ex: Ethernet_2.
                var interfaceName = netAdapter["InterfaceName"]?.ToString();

                //The interface type as defined by the Internet Assigned Names Authority (IANA).
                //https://www.iana.org/assignments/ianaiftype-mib/ianaiftype-mib
                var interfaceType = Convert.ToUInt32(netAdapter["InterfaceType"]);

                //The types of physical media that the network adapter supports.
                var ndisPhysicalMedium = Convert.ToUInt32(netAdapter["NdisPhysicalMedium"]);

                if (string.IsNullOrEmpty(interfaceName) 
                    //ethernetCsmacd(6) --for all ethernet-like interfaces, regardless of speed, as per RFC3635
                    || interfaceType != 6
                    //802.3
                    || ndisPhysicalMedium != 0 && ndisPhysicalMedium != 14)
                {
                    continue;
                }
                
                try
                {
                    (netAdapter as ManagementObject)?.InvokeMethod("Restart", null);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error while restarting adapter");
                }
            }
        }
    }
}