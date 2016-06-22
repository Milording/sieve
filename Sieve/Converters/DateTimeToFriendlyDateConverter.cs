using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Sieve.Converters
{
    class DateTimeToFriendlyDateConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var date = value as DateTime?;
            return String.Format("{0:M}", date);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
