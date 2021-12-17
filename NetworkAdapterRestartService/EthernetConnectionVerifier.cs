using System.Linq;
using System.Net.NetworkInformation;
using NetworkAdapterRestartService.Interfaces;

namespace NetworkAdapterRestartService
{
    internal sealed class EthernetConnectionVerifier : IConnectionVerifier
    {
        public bool VerifyConnectionDown()
        {
            var ethernetInterface = NetworkInterface
                .GetAllNetworkInterfaces()
                .SingleOrDefault(_ => _.NetworkInterfaceType == NetworkInterfaceType.Ethernet);
            
            return ethernetInterface is null || ethernetInterface.OperationalStatus == OperationalStatus.Down;
        }
    }
}