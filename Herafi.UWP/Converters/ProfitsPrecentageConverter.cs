using Herafi.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Herafi.UWP.Converters
{
    class ProfitsPrecentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (parameter as string == "Color")
            {
                if (double.Parse(value as string) > 0)
                {
                    if (Settings.Theme == Choices.ChooseTheme(Themes.Light))
                    {
                        return new SolidColorBrush((Color)App.Current.Resources["FixedTextBlockForegroundColorLight"]);
                    }
                    else if (Settings.Theme == Choices.ChooseTheme(Themes.Dark))
                    {
                        return new SolidColorBrush((Color)App.Current.Resources["FixedTextBlockForegroundColorDark"]);
                    }
                    else
                    {
                        return App.Current.Resources["FixedTextBlockForegroundBrush"] as SolidColorBrush;
                    }
                }
                else if (double.Parse(value as string) < 0)
                {
                    if (Settings.Theme == Choices.ChooseTheme(Themes.Light))
                    {
                        return new SolidColorBrush((Color)App.Current.Resources["ErrorTextBlockForegroundColorLight"]);
                    }
                    else if (Settings.Theme == Choices.ChooseTheme(Themes.Dark))
                    {
                        return new SolidColorBrush((Color)App.Current.Resources["ErrorTextBlockForegroundColorDark"]);
                    }
                    else
                    {
                        return App.Current.Resources["ErrorTextBlockForegroundBrush"] as SolidColorBrush;
                    }
                }
                else /* double.Parse(value as string) == 0 */
                {
                    if (Settings.Theme == Choices.ChooseTheme(Themes.Light))
                    {
                        return new SolidColorBrush((Color)App.Current.Resources["AccentTextBlockForegroundColorLight"]);
                    }
                    else if (Settings.Theme == Choices.ChooseTheme(Themes.Dark))
                    {
                        return new SolidColorBrush((Color)App.Current.Resources["AccentTextBlockForegroundColorDark"]);
                    }
                    else
                    {
                        return App.Current.Resources["AccentTextBlockForegroundBrush"] as SolidColorBrush;
                    }
                }
            }
            else //Icon
            {
                if (double.Parse(value as string) > 0)
                {
                    return App.Current.Resources["IncreasingProfitsArrow"] as string;
                }
                else if (double.Parse(value as string) < 0)
                {
                    return App.Current.Resources["DecreasingProfitsArrow"] as string;
                }
                else /* double.Parse(value as string) == 0 */
                {
                    return App.Current.Resources["EqualsProfitsArrow"] as string;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
