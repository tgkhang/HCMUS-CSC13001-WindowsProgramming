using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace POS_For_Small_Shop.Converters
{
    //converters to handle UI state changes
    /// <summary>
    /// Show UI when null, 
    /// </summary>
    public class NullToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value == null ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }


    //show element when not null
    public class NotNullToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value != null ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

