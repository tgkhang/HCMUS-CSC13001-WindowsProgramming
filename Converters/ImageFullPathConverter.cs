using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;


namespace POS_For_Small_Shop.Converters
{
    class ImageFullPathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return null; // or return a default path if preferred
            }

            if (value is string relativePath)
            {
                string rootPath = AppContext.BaseDirectory;
                string fullPath = Path.Combine(rootPath, relativePath);
               
                return fullPath;
            }

            // Return unchanged value or throw exception if type is unexpected
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException("ConvertBack is not supported for this converter");
        }
    }
}
