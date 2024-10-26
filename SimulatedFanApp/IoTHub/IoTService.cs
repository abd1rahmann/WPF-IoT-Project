using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SimulatedFanApp.IoTHub
{
    public class IoTService
    {
        private DeviceClient deviceClient;
        private string? connectionString;
        private string? deviceId;

        public IoTService()
        {
            LoadSettings();
            deviceClient = DeviceClient.CreateFromConnectionString(connectionString, TransportType.Mqtt);
        }

        private void LoadSettings()
        {
            try
            {
                string json = File.ReadAllText("settings.json");
                dynamic settings = JsonConvert.DeserializeObject(json);

                connectionString = settings.ConnectionString;
                deviceId = settings.DeviceId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fel vid laddning av inställningar: {ex.Message}");
            }
        }

        public async Task InitializeDeviceClientAsync()
        {
            await deviceClient.SetDesiredPropertyUpdateCallbackAsync(OnDesiredPropertyChanged, null);
        }

        private Task OnDesiredPropertyChanged(TwinCollection desiredProperties, object userContext)
        {
            if (desiredProperties.Contains("FanStatus"))
            {
                string fanStatus = desiredProperties["FanStatus"];
                Console.WriteLine($"Desired FanStatus: {fanStatus}");

                if (fanStatus == "On")
                {
                    TurnOnFan();
                }
                else if (fanStatus == "Off")
                {
                    TurnOffFan();
                }
            }

            return Task.CompletedTask;
        }

        private void TurnOnFan()
        {
            Console.WriteLine("Fan is turned ON");
            // Logik för att starta fläkten
        }

        private void TurnOffFan()
        {
            Console.WriteLine("Fan is turned OFF");
            // Logik för att stänga av fläkten
        }

        public async Task ReportFanStatusAsync(string fanStatus)
        {
            var reportedProperties = new TwinCollection();
            reportedProperties["FanStatus"] = fanStatus;

            await deviceClient.UpdateReportedPropertiesAsync(reportedProperties);
            Console.WriteLine($"Reported FanStatus: {fanStatus}");
        }

        public async Task SendMessageToIoTHub(string message)
        {
            if (deviceClient == null)
            {
                throw new InvalidOperationException("DeviceClient är inte ansluten till IoT Hub. Kontrollera anslutningen.");
            }

            var cloudMessage = new Message(Encoding.UTF8.GetBytes(message))
            {
                ContentType = "application/json",
                ContentEncoding = "utf-8"
            };

            try
            {
                await deviceClient.SendEventAsync(cloudMessage);
                Console.WriteLine($"Meddelande skickat från {deviceId}: {message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fel vid sändning av meddelande: {ex.Message}");
            }
        }
    }
}
