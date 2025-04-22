using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI;

namespace POS_For_Small_Shop.Converters
{
    public class ValueToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            float numericValue;
            if (value is float floatValue)
                numericValue = floatValue;
            else if (value is int intValue)
                numericValue = intValue;
            else
                return new SolidColorBrush(Colors.Gray); // Default fallback

            if (numericValue < 0.0)
                return new SolidColorBrush(Colors.Red);
            else if (numericValue > 0.0)
                return new SolidColorBrush(Colors.Green);
            else
                return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class ValueToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            float numericValue;
            if (value is float floatValue)
                numericValue = floatValue;
            else if (value is int intValue)
                numericValue = intValue;
            else
                return value?.ToString() ?? string.Empty; // Fallback for non-numeric values

            if (numericValue == 0)
                return "0";

            float displayValue = Math.Abs(numericValue) >= 1000 ? numericValue / 1000 : numericValue;
            string sign = numericValue > 0 ? "+" : numericValue < 0 ? "" : "";

            if (Math.Abs(numericValue) >= 1000)
                return $"{sign}{displayValue:F3}";
            else
                return $"{sign}{displayValue:F0}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

