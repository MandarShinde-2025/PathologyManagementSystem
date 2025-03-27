namespace PatientService.Services;

public interface IRedisPubSubService
{
    Task PublishTestRequestAsync(string message);
    Task ConsumeTestRequestAsync(Action<string> messageHandler);
}