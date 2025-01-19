using StackExchange.Redis;

namespace NotificationService.Services
{
    public class NotificationPublisher
    {
        private readonly IConnectionMultiplexer _redis;

        public NotificationPublisher(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task PublishNotification(string channel, string message)
        {
            var publisher = _redis.GetSubscriber();
            await publisher.PublishAsync(channel, message);
        }
    }
}