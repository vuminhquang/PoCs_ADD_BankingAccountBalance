using System.Text.Json;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using Contract;

namespace BusinessApplication
{
    public class RedisBackgroundService(
        ITransactionProcessor transactionProcessor,
        RedisConfiguration redisConfiguration,
        IConnectionMultiplexer redis)
        : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var subscriber = redis.GetSubscriber();
            await subscriber.SubscribeAsync(redisConfiguration.Channel, async (channel, message) =>
            {
                var transactionEvent = JsonSerializer.Deserialize<TransactionCreatedEvent>(message);
                var transactionDto = new TransactionDto
                {
                    AccountId = transactionEvent.AccountId,
                    Amount = transactionEvent.Amount,
                    // Assume transaction type is used internally as needed
                    Timestamp = DateTime.UtcNow
                };

                await transactionProcessor.ProcessTransactionAsync(transactionDto);
            });

            // Keep listening until cancellation is requested
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}