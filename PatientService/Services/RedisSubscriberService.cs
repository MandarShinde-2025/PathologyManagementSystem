
using Microsoft.AspNetCore.SignalR;
using PatientService.SignalR;

namespace PatientService.Services;

public class RedisSubscriberService : BackgroundService
{
    private readonly IRedisPubSubService _redisPubSubService;
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly string _channelName;

    public RedisSubscriberService(IHubContext<NotificationHub> hubContext,
        IRedisPubSubService redisPubSubService,
        IConfiguration configuration)
    {
        _redisPubSubService = redisPubSubService;
        _hubContext = hubContext;
        _channelName = configuration["Redis:Channel"]!;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _redisPubSubService.ConsumeTestRequestAsync(async message =>
        {
            await NotifyCollectorAsync(message);
        });
    }

    private async Task NotifyCollectorAsync(string message)
    {
        await _hubContext.Clients.All.SendAsync(_channelName, message);
        Console.WriteLine($"[Collector Notified]: {message}");
        await Task.CompletedTask;
    }
}