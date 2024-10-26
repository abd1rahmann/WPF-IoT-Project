using System.Configuration;
using System.ComponentModel;
using System.Windows.Input;


public class SettingsViewModel : ViewModelBase
{
    private string _connectionString;
    private string _deviceId;
    private string _autoShutoffTime;

    public string ConnectionString
    {
        get => _connectionString;
        set
        {
            _connectionString = value;
            OnPropertyChanged(nameof(ConnectionString));
        }
    }

    public string DeviceId
    {
        get => _deviceId;
        set
        {
            _deviceId = value;
            OnPropertyChanged(nameof(DeviceId));
        }
    }

    
    public string AutoShutoffTime
    {
        get => _autoShutoffTime;
        set
        {
            _autoShutoffTime = value;
            OnPropertyChanged(nameof(AutoShutoffTime));
        }
    }

    public ICommand SaveSettingsCommand { get; }
    public ICommand SaveAutoShutoffTimeCommand { get; }

    public SettingsViewModel()
    {
        _connectionString = ConfigurationManager.AppSettings["IoTHubConnectionString"];
        _deviceId = ConfigurationManager.AppSettings["DeviceId"];
        _autoShutoffTime = "30 minuter";

        SaveSettingsCommand = new RelayCommand(SaveSettings);
        SaveAutoShutoffTimeCommand = new RelayCommand(SaveAutoShutoffTime);
    }

    private void SaveSettings()
    {
        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        config.AppSettings.Settings["IoTHubConnectionString"].Value = ConnectionString;
        config.AppSettings.Settings["DeviceId"].Value = DeviceId;
        config.Save(ConfigurationSaveMode.Modified);
        ConfigurationManager.RefreshSection("appSettings");
    }

   

    private void SaveAutoShutoffTime()
    {
        // Här kan du spara auto shutoff tid om det behövs
        // Just nu sparas den inte, men du kan använda den vid behov
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
