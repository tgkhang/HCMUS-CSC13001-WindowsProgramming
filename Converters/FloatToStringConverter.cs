using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_For_Small_Shop.Converters
{
    public class FloatToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is float floatValue)
            {
                return floatValue.ToString(CultureInfo.InvariantCulture);
            }
            return "0"; // Default if value is null
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string stringValue && float.TryParse(stringValue, NumberStyles.Float, CultureInfo.InvariantCulture, out float result))
            {
                return result;
            }
            return 0f; // Default if parsing fails
        }
    }
}
