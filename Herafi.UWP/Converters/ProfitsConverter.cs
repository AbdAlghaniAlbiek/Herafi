using Herafi.Core.Helpers;
using Herafi.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Herafi.UWP.Converters
{
    public class ProfitsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (parameter as string == "Color")
            {
                var temp = value as ObservableCollection<Profit>;

                if ((double.Parse(temp[temp.Count - 1].Paids) - double.Parse(temp[temp.Count - 2].Paids)) > 0)
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
                else if ((double.Parse(temp[temp.Count - 1].Paids) - double.Parse(temp[temp.Count - 2].Paids)) < 0)
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
                else
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


            else  //Value
            {
                ObservableCollection<ModifiedProfit> modProfits =
               new ObservableCollection<ModifiedProfit>();

                for (int i = 0; i < (value as ObservableCollection<Profit>).Count; i++)
                {
                    modProfits.Add(new ModifiedProfit
                    {
                        Day = (value as ObservableCollection<Profit>)[i].Day,
                        Paids = double.Parse((value as ObservableCollection<Profit>)[i].Paids)
                    });
                }

                //Very important to clear memeory
                (value as ObservableCollection<Profit>).Clear();

                return modProfits;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
