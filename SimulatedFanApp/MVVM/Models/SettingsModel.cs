using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulatedFanApp.MVVM.Models
{
    public class SettingsModel
    {
        public string ConnectionString
        {
            get => ConfigurationManager.AppSettings["IoTHubConnectionString"];
            set
            {
                UpdateAppSetting("IoTHubConnectionString", value);
            }
        }

        public string DeviceId
        {
            get => ConfigurationManager.AppSettings["DeviceId"];
            set
            {
                UpdateAppSetting("DeviceId", value);
            }
        }

        private void UpdateAppSetting(string key, string value)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            if (settings[key] == null)
            {
                settings.Add(key, value);
            }
            else
            {
                settings[key].Value = value;
            }
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
        }
    }
}
