using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_For_Small_Shop.Converters
{
    public class StatusIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value?.ToString().ToLower() switch
            {
                "closed" => Symbol.Accept,
                "active" or "open" => Symbol.Clock,
                _ => Symbol.OutlineStar
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }

    public class StatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value?.ToString().ToLower() switch
            {
                "closed" or "completed" => new SolidColorBrush(Colors.Green),
                "open" or "active" or "canceled" => new SolidColorBrush(Colors.Red),
                "pending" => new SolidColorBrush(Colors.LightGray),
                _ => new SolidColorBrush(Colors.Gray)
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
