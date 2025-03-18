using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;

namespace POS_For_Small_Shop.Converters
{
  
    public class OrderStringFormatter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // Default behavior: just return the value if it's null or empty
            if (value == null) return string.Empty;

            // The 'parameter' is the static part of the string (e.g., "Payment: ")
            string format = parameter as string;
            if (string.IsNullOrEmpty(format))
            {
                return value.ToString();
            }

            // Format the string with the value
            return string.Format(format, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
