using SimulatedFanApp.IoTHub;
using System.Threading.Tasks;

public class MainViewModel
{
    private IoTService iotService;

    public MainViewModel()
    {
        iotService = new IoTService();
        Task.Run(async () => await iotService.InitializeDeviceClientAsync());
    }

    public async Task UpdateFanStatus(string status)
    {
        await iotService.ReportFanStatusAsync(status);
    }

    public async Task SendMessage(string message)
    {
        await iotService.SendMessageToIoTHub(message);
    }
}
