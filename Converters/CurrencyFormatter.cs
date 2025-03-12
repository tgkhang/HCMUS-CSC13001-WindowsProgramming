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
            throw new NotImplementedException();
        }
    }
}