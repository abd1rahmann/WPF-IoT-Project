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

            deviceClient = DeviceClient.CreateFromConnectionString(connectionString, TransportType.Mqtt);

            fanModel = new FanModel(connectionString, deviceId);

            InitializeDeviceClientAsync();

            ToggleFanCommand = new RelayCommand(async (obj) => await ToggleFanAsync());
            
        }

        private async Task ToggleFanAsync()
        {
            if (IsRunning)
            {
                await fanModel.StopFanAsync(); 
            }
            else
            {
                await fanModel.StartFanAsync();  
            }

            OnPropertyChanged(nameof(IsRunning));
        }

        private async Task InitializeDeviceClientAsync()
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
            IsRunning = true;
        }

        private void TurnOffFan()
        {
            IsRunning = false;
        }

        private async Task ReportFanStatusAsync(string fanStatus)
        {
            var reportedProperties = new TwinCollection();
            reportedProperties["FanStatus"] = fanStatus;

            await deviceClient.UpdateReportedPropertiesAsync(reportedProperties);
            Console.WriteLine($"Reported FanStatus: {fanStatus}");
        }
    }
}
