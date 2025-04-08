using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;
namespace POS_For_Small_Shop.Converters
{
    public class CurrencyFormatter : IValueConverter
    {
        private static readonly CultureInfo _vietnameseCulture = new CultureInfo("vi-VN");
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return string.Empty;
            try
            {

                decimal amount = System.Convert.ToDecimal(value);
                return string.Format(_vietnameseCulture, "{0:#,##0} đ", amount);
            }
            catch
            {
                return value.ToString();
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return 0m;

            string strValue = value.ToString();

            // Remove the currency symbol (đ) and any spaces
            strValue = strValue.Replace("đ", "").Trim();

            // Remove thousands separators - in Vietnamese format, the period is used as thousands separator
            strValue = strValue.Replace(".", "");

            try
            {
                decimal result = decimal.Parse(strValue, _vietnameseCulture);

                // Convert to the target type (likely decimal, double, int, etc.)
                if (targetType == typeof(decimal))
                    return result;
                else if (targetType == typeof(double))
                    return (double)result;
                else if (targetType == typeof(int))
                    return (int)result;
                else if (targetType == typeof(long))
                    return (long)result;
                else if (targetType == typeof(string))
                    return result.ToString();
                else
                    return System.Convert.ChangeType(result, targetType);
            }
            catch
            {
                // Return default value for the target type
                return targetType == typeof(string) ? string.Empty : Activator.CreateInstance(targetType);
            }
        }
    }
}