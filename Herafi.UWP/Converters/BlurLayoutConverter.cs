using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Herafi.UWP.Converters
{
    public class BlurLayoutConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return System.Convert.ToDouble(value) + 36.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
