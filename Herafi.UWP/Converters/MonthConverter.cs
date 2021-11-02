using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Herafi.UWP.Converters
{
    public class MonthConverter : IValueConverter
    {
       

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                ObservableCollection<string> modifidMonths =
               new ObservableCollection<string>();

                for (int i = 0; i < (value as ObservableCollection<string>).Count; i++)
                {
                    switch ((value as ObservableCollection<string>)[i])
                    {
                        case "1":
                            {
                                modifidMonths.Add("January");
                                break;
                            }
                        case "2":
                            {
                                modifidMonths.Add("February");
                                break;
                            }
                        case "3":
                            {
                                modifidMonths.Add("March");
                                break;
                            }
                        case "4":
                            {
                                modifidMonths.Add("April");
                                break;
                            }
                        case "5":
                            {
                                modifidMonths.Add("May");
                                break;
                            }
                        case "6":
                            {
                                modifidMonths.Add("June");
                                break;
                            }
                        case "7":
                            {
                                modifidMonths.Add("July");
                                break;
                            }
                        case "8":
                            {
                                modifidMonths.Add("August");
                                break;
                            }
                        case "9":
                            {
                                modifidMonths.Add("September");
                                break;
                            }
                        case "10":
                            {
                                modifidMonths.Add("October");
                                break;
                            }
                        case "11":
                            {
                                modifidMonths.Add("November");
                                break;
                            }
                        case "12":
                            {
                                modifidMonths.Add("December");
                                break;
                            }
                        default:
                            break;
                    }
                }

            (value as ObservableCollection<string>).Clear();

                return modifidMonths;
            }

            return null;
        }
            

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
