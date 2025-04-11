using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_For_Small_Shop.Converters
{
    public class DateTimeToDateTimeOffsetConverter : IValueConverter
    {
        // Convert from DateTime (source) to DateTimeOffset (target)
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime dateTime)
            {
                if (dateTime == DateTime.MinValue)
                {
                    return null;
                }
                return new DateTimeOffset(dateTime);
            }
            return null;
        }

        // Convert back from DateTimeOffset (target) to DateTime (source)
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTimeOffset dateTimeOffset)
            {
                if (parameter?.ToString() == "EndDate")
                {
                    return new DateTime(dateTimeOffset.Year, dateTimeOffset.Month, dateTimeOffset.Day, 23, 59, 59);
                }
                return new DateTime(dateTimeOffset.Year, dateTimeOffset.Month, dateTimeOffset.Day, 0, 0, 0);
            }
            return DateTime.MinValue;
        }
    }
}
