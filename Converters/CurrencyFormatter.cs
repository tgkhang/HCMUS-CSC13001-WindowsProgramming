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
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return "";
            }

            int ammount = (int)value;
            CultureInfo culture = new CultureInfo("vi-VN");
            string formatted = ammount.ToString("#,### đ", culture.NumberFormat);
            return formatted;
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
