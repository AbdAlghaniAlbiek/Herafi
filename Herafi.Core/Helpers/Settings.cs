using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Herafi.Core.Helpers
{
    public class Settings
    {
        private static ApplicationDataContainer _localSettings = 
            ApplicationData.Current.LocalSettings;

        public static string AdminId
        {
            set { _localSettings.Values["adminId"] = value; }
            get => _localSettings.Values.ContainsKey("adminId") ? _localSettings.Values["adminId"] as string : string.Empty;
        }

        public static string FacebookId
        {
            set { _localSettings.Values["facebookId"] = value; }
            get => _localSettings.Values.ContainsKey("facebookId") ? _localSettings.Values["facebookId"] as string : string.Empty;
        }

        public static string MicrosoftId
        {
            set { _localSettings.Values["microsoftId"] = value; }
            get => _localSettings.Values.ContainsKey("microsoftId") ? _localSettings.Values["microsoftId"] as string : string.Empty;
        }

        public static bool SoundEnabled
        {
            set { _localSettings.Values["Sound"] = value; }
            get => _localSettings.Values.ContainsKey("Sound") ? (bool)_localSettings.Values["Sound"] : false;
        }

        public static bool SpatialAudio
        {
            set { _localSettings.Values["spatialAudio"] = value; }
            get => _localSettings.Values.ContainsKey("spatialAudio") ? (bool)_localSettings.Values["spatialAudio"] : false;
        }

        public static string Language
        {
            set { _localSettings.Values["Language"] = value; }
            get => _localSettings.Values.ContainsKey("Language") ? (string)_localSettings.Values["Language"] : Choices.ChooseLanguage(Languages.English);
        }

        public static string Theme
        {
            set { _localSettings.Values["Theme"] = value; }
            get => _localSettings.Values.ContainsKey("Theme") ? (string)_localSettings.Values["Theme"] : Choices.ChooseTheme(Themes.System);
        }
    }
}
