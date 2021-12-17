using System;
using System.Management;

// var x = NetworkInterface.GetAllNetworkInterfaces().Single(_ => _.NetworkInterfaceType == NetworkInterfaceType.Ethernet);
// // OperationalStatus of Down (2) and Up (1)
// Console.WriteLine(x.OperationalStatus);

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

    if (string.IsNullOrEmpty(interfaceName) || interfaceType != 6 ||
        (ndisPhysicalMedium != 0 && ndisPhysicalMedium != 14))
    {
        continue;
    }

    try
    {
        (netAdapter as ManagementObject)?.InvokeMethod("Restart", null);
    }
    catch (Exception)
    {
        // ignored
    }
}