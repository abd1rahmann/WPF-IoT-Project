namespace SimulatedFanApp.MVVM.ViewModels
{
    public class ViewModelLocator
    {
        public MainViewModel MainViewModel { get; } = new MainViewModel();

        public SettingsViewModel SettingsViewModel { get; } = new SettingsViewModel();

        public FanViewModel FanViewModel { get; } = new FanViewModel();
    }
}
