using StackExchange.Redis;

namespace PatientService.Services;

public class RedisPubSubService: IRedisPubSubService
{
    private readonly ConnectionMultiplexer _redis;

    private readonly string _channelName;

    public RedisPubSubService(IConfiguration configuration)
    {
        _redis = ConnectionMultiplexer.Connect(configuration["Redis:ConnectionString"]!);
        _channelName = configuration["Redis:Channel"]!;
    }

    public async Task PublishTestRequestAsync(string message)
    {
        var _subscriber = _redis.GetSubscriber();
        await _subscriber.PublishAsync(_channelName, message);
    }

    public async Task ConsumeTestRequestAsync(Action<string> messageHandler)
    {
        var _subscriber = _redis.GetSubscriber();
        await _subscriber.SubscribeAsync(_channelName, (channel, message) =>
        {
            messageHandler?.Invoke(message!);
        });
    }
}