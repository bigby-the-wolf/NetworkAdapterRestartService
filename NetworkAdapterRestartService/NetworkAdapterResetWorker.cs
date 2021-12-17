using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetworkAdapterRestartService.Interfaces;
using static System.Threading.Tasks.Task;

namespace NetworkAdapterRestartService
{
    public class NetworkAdapterResetWorker : BackgroundService
    {
        private readonly ILogger<NetworkAdapterResetWorker> _logger;
        private readonly IConnectionValidator _connectionValidator;

        public NetworkAdapterResetWorker(ILogger<NetworkAdapterResetWorker> logger, IConnectionValidator connectionValidator)
        {
            _logger = logger;
            _connectionValidator = connectionValidator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Service started");

            await Delay(10000, stoppingToken);

            await _connectionValidator.ValidateConnection();

            _logger.LogInformation("Stopping service.");

            await StopAsync(stoppingToken);
        }
    }
}