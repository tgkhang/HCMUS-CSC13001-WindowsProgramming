using System;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Media.Imaging;

namespace POS_For_Small_Shop.Services
{
    public interface IQRService
    {
        /// <summary>
        /// Generates a QR code image for payment based on the specified amount and order information
        /// </summary>
        /// <param name="amount">The payment amount</param>
        /// <param name="orderInfo">Additional order information (limited to 25 characters)</param>
        /// <returns>A BitmapImage containing the QR code</returns>
        Task<BitmapImage> GenerateQRCodeImageAsync(decimal amount, string orderInfo);
    }
}