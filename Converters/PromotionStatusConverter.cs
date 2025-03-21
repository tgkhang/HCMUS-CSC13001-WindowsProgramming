using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_For_Small_Shop.Data.Models;
using System.Diagnostics;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Hosting;

namespace POS_For_Small_Shop.Converters
{
    public class PromotionStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Promotion promo)
            {
                string status = GetStatus(promo);
                string param = parameter?.ToString();

                // Determine what to return based on targetType and parameter
                if (targetType == typeof(Brush)) // For Background
                {
                    if (param == "Stripe")
                    {
                        // Alternate background colors based on PromoID for striping
                        //int index = GetItemIndex(promo);
                        return promo.PromoID % 2 == 0
                            ? new SolidColorBrush(Colors.White)
                            : new SolidColorBrush(Windows.UI.Color.FromArgb(255, 245, 245, 245)); 
                    }
                    // Existing status-based background logic
                    return status.ToLower() switch
                    {
                        "upcoming" => new SolidColorBrush(Colors.DarkOrange),
                        "active" => new SolidColorBrush(Colors.Green),
                        "expired" => new SolidColorBrush(Colors.Red),
                        _ => new SolidColorBrush(Colors.Gray)
                    };
                }
                else if (targetType == typeof(string)) // For Text
                {
                    if (param == "Status")
                        return status;
                    if (param == "TimeRemaining")
                        return GetTimeRemaining(promo);
                }
            }
            // Fallbacks
            if (targetType == typeof(Brush))
                return new SolidColorBrush(Colors.Gray);
            return string.Empty; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }



        public static string GetStatus(Promotion promo)
        {
            if (DateTime.Now < promo.StartDate)
                return "Upcoming";
            else if (DateTime.Now < promo.EndDate)
                return "Active";
            else
                return "Expired";
        }

        public static string GetTimeRemaining(Promotion promo)
        {
            if (DateTime.Now < promo.StartDate)
            {
                var untilStart = promo.StartDate - DateTime.Now;
                return $"Starts in {untilStart.Days}d {untilStart.Hours}h {untilStart.Minutes}m {untilStart.Seconds}s";
            }
            else if (DateTime.Now < promo.EndDate)
            {
                var remaining = promo.EndDate - DateTime.Now;
                return $"{remaining.Days}d {remaining.Hours}h {remaining.Minutes}m {remaining.Seconds}s remaining";
            }
            else
            {
                return "Expired";
            }
        }
    }
}
