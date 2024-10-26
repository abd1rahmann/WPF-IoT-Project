using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimulatedFanApp.MVVM.ViewModels
{
    public class FanViewModel : ViewModelBase
    {
        private readonly FanModel fanModel;
        private readonly DeviceClient deviceClient;

        public bool IsRunning
        {
            get => fanModel.IsRunning;
            set
            {
                fanModel.IsRunning = value;
                OnPropertyChanged(nameof(IsRunning));
                // Rapportera det aktuella tillståndet till device twin
                ReportFanStatusAsync(fanModel.IsRunning ? "On" : "Off");
            }
        }

        public ICommand ToggleFanCommand { get; }

        public FanViewModel()
        {
            var main = new MainWindow();
            
            var connectionString = ConfigurationManager.AppSettings["IoTHubConnectionString"];
            if (string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine("Connection string is null or empty!");
            }

            var deviceId = ConfigurationManager.AppSettings["DeviceId"];
            if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(deviceId))
            {
                throw new ArgumentNullException("Connection string or device ID cannot be null.");
            }

            // Skapa IoT Hub-anslutningen med din connection string
            deviceClient = DeviceClient.CreateFromConnectionString(connectionString, TransportType.Mqtt);

            fanModel = new FanModel(connectionString, deviceId);

            // Sätt callback för att lyssna på device twin-uppdateringar
            InitializeDeviceClientAsync();

            // Skapa ett kommando och bind metoden ToggleFan
            ToggleFanCommand = new RelayCommand(async (obj) => await ToggleFanAsync());
            
        }

        // Asynkron metod för att slå på/stänga av fläkten
        private async Task ToggleFanAsync()
        {
            if (IsRunning)
            {
                await fanModel.StopFanAsync();  // Gör StopFan asynkron
            }
            else
            {
                await fanModel.StartFanAsync();  // Gör StartFan asynkron
            }

            // Meddela att IsRunning har ändrats
            OnPropertyChanged(nameof(IsRunning));
        }

        // Asynkron metod för att hantera uppdateringar från device twin
        private async Task InitializeDeviceClientAsync()
        {
            // Callback för när desired properties uppdateras i device twin
            await deviceClient.SetDesiredPropertyUpdateCallbackAsync(OnDesiredPropertyChanged, null);
        }

        // Callback-metod för att hantera ändringar av desired properties
        private Task OnDesiredPropertyChanged(TwinCollection desiredProperties, object userContext)
        {
            if (desiredProperties.Contains("FanStatus"))
            {
                string fanStatus = desiredProperties["FanStatus"];
                Console.WriteLine($"Desired FanStatus: {fanStatus}");

                // Uppdatera fläktens tillstånd baserat på det önskade tillståndet
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

        // Metod för att slå på fläkten
        private void TurnOnFan()
        {
            IsRunning = true;
        }

        // Metod för att stänga av fläkten
        private void TurnOffFan()
        {
            IsRunning = false;
        }

        // Asynkron metod för att rapportera fläktens status till device twin
        private async Task ReportFanStatusAsync(string fanStatus)
        {
            var reportedProperties = new TwinCollection();
            reportedProperties["FanStatus"] = fanStatus;

            await deviceClient.UpdateReportedPropertiesAsync(reportedProperties);
            Console.WriteLine($"Reported FanStatus: {fanStatus}");
        }
    }
}
