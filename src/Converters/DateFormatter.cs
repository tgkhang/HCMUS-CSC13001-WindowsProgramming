using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;



namespace POS_For_Small_Shop.Converters
{
    public class DateFormatter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime date)
            {
                return date.ToString("yyyy-MM-dd");
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (DateTime.TryParse(value as string, out DateTime date))
            {
                return date;
            }
            return null;
        }
    }
}
