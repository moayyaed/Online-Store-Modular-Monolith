using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Modules;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Common.Messaging.Transport.InMemory
{
    internal sealed class InMemoryBackgroundDispatcher : BackgroundService
    {
        private readonly IMessageChannel _channel;
        private readonly ILogger<InMemoryBackgroundDispatcher> _logger;
        private readonly IModuleClient _moduleClient;

        public InMemoryBackgroundDispatcher(IMessageChannel channel, IModuleClient moduleClient,
            ILogger<InMemoryBackgroundDispatcher> logger)
        {
            _channel = channel;
            _moduleClient = moduleClient;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Running the background event dispatcher...");
            await foreach (var @event in _channel.Reader.ReadAllAsync(stoppingToken))
                try
                {
                    await _moduleClient.PublishAsync(@event);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, exception.Message);
                }

            _logger.LogInformation("Finished running the background event dispatcher.");
        }
    }
}