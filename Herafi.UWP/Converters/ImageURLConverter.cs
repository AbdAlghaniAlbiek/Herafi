using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Herafi.Core.Security;

namespace Herafi.UWP.Converters
{
    public class ImageURLConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is null))
            {
                if (value.ToString().Split("/").Length > 1)
                {
                    if ((parameter as string).Split("-")[0] == "ImageEx")
                    {
                        return value as string;
                    }
                    else
                    {
                        return new BitmapImage(
                                !string.IsNullOrEmpty(value as string) ? new Uri(value as string) : null);
                    }
                }
                else
                {
                    if ((parameter as string).Split("-")[0] == "ImageEx")
                    {
                        if ((parameter as string).Split("-")[1] == "Admin")
                        {
                            return Common.ADMIN_IMAGES_PATH + (value as string);
                        }
                        else if ((parameter as string).Split("-")[1] == "Craftman")
                        {
                            return Common.CRAFTMEN_IMAGES_PATH + (value as string);
                        }
                        else if ((parameter as string).Split("-")[1] == "User")
                        {
                            return Common.USERS_IMAGES_PATH + (value as string);
                        }
                    }
                    else
                    {
                        if ((parameter as string).Split("-")[1] == "Admin")
                        {
                            return new BitmapImage(
                                !string.IsNullOrEmpty(value as string) ? new Uri(Common.USERS_IMAGES_PATH + value as string) : null);
                        }
                        else if ((parameter as string).Split("-")[1] == "Craftman")
                        {
                            return new BitmapImage(
                                !string.IsNullOrEmpty(value as string) ? new Uri(Common.USERS_IMAGES_PATH + value as string) : null);
                        }
                        else if ((parameter as string).Split("-")[1] == "User")
                        {
                            return new BitmapImage(
                                !string.IsNullOrEmpty(value as string) ? new Uri(Common.USERS_IMAGES_PATH + value as string) : null);
                        }
                    }
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
