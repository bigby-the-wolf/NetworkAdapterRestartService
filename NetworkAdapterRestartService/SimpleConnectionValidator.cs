using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NetworkAdapterRestartService.Interfaces;

namespace NetworkAdapterRestartService
{
    internal sealed class SimpleConnectionValidator : IConnectionValidator
    {
        private readonly IConnectionVerifier _connectionVerifier;
        private readonly INetworkAdapterProvider _networkAdapterProvider;
        private readonly ILogger<SimpleConnectionValidator> _logger;

        public SimpleConnectionValidator(
            IConnectionVerifier connectionVerifier,
            INetworkAdapterProvider networkAdapterProvider,
            ILogger<SimpleConnectionValidator> logger)
        {
            _connectionVerifier = connectionVerifier;
            _networkAdapterProvider = networkAdapterProvider;
            _logger = logger;
        }

        public async Task ValidateConnection()
        {
            while (_connectionVerifier.VerifyConnectionDown())
            {
                _logger.LogInformation("Connection is down, attempting to restart ethernet adapter.");
                
                _networkAdapterProvider.ResetAdapter();

                await Task.Delay(10000);
            }
            
            _logger.LogInformation("Connection is up.");
        }
    }
}