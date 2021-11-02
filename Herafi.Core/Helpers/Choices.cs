using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herafi.Core.Helpers
{
    public class Choices
    {
        public static string ChooseLanguage(Languages languages)
        {
            switch (languages)
            {
                case Languages.Arabic:
                    {
                        return "Arabic";
                    }
                case Languages.English:
                    {
                        return "English";
                    }
                default:
                    return string.Empty;
            }
        }

        public static string ChooseTheme(Themes themes)
        {
            switch (themes)
            {
                case Themes.System:
                    {
                        return "System";
                    }

                case Themes.Light:
                    {
                        return "Light";
                    }
                case Themes.Dark:
                    {
                        return "Dark";
                    }
                default:
                    return string.Empty;
            }
        }

        public static string ChooseAppEnvironment(AppEnvironment appEnvironment)
        {
            switch (appEnvironment)
            {
                case AppEnvironment.Development:
                    {
                        return "Development";
                    }
                case AppEnvironment.Production:
                    {
                        return "Production";
                    }
                default:
                    return string.Empty;
            }
        }
    }

    public enum AppEnvironment
    {
        Development,
        Production
    }

    public enum Languages
    {
        Arabic,
        English
    }

    public enum Themes
    {
        System,
        Light,
        Dark
    }
}
