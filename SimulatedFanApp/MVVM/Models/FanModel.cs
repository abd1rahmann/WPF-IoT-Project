using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TransportType = Microsoft.Azure.Devices.Client.TransportType;

public class FanModel(string connectionString, string deviceId)
{
    public bool IsRunning { get; set; }

    private DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(connectionString, TransportType.Mqtt);
    private string connectionString = connectionString;
    private string deviceId = deviceId;

    public async Task StartFanAsync()
    {
        IsRunning = true;
        await ReportStateAsync();
    }

    public async Task StopFanAsync()
    {
        IsRunning = false;
        await ReportStateAsync();
    }

    private async Task ReportStateAsync()
    {
        var twinProperties = new
        {
            isRunning = IsRunning
        };

        var messageString = JsonConvert.SerializeObject(twinProperties);
        var message = new Message(Encoding.ASCII.GetBytes(messageString));

        await deviceClient.SendEventAsync(message);  // Rapportera till device-twin
    }
}
