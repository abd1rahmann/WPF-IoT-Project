namespace SimulatedFanApp.MVVM.ViewModels
{
    public class ViewModelLocator
    {
        // Property to access MainViewModel
        public MainViewModel MainViewModel { get; } = new MainViewModel();

        // Property to access SettingsViewModel (optional)
        public SettingsViewModel SettingsViewModel { get; } = new SettingsViewModel();

        // Property to access FanViewModel (optional)
        public FanViewModel FanViewModel { get; } = new FanViewModel();
    }
}
