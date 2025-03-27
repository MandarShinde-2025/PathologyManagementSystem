using Microsoft.AspNetCore.SignalR;

namespace PatientService.SignalR;

public class NotificationHub : Hub
{
    public async Task SendTestRequest(string message)
    {
        await Clients.All.SendAsync("ReceiveTestRequest", message);
    }
}