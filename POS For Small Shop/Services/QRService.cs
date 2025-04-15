using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using System.Diagnostics;

namespace POS_For_Small_Shop.Services
{
    public class QRService : IQRService
    {
        public async Task<BitmapImage> GenerateQRCodeImageAsync(decimal amount, string orderInfo)
        {
            try
            {
                // Limit order info to 25 characters as per API requirements
                if (orderInfo?.Length > 25)
                {
                    orderInfo = orderInfo.Substring(0, 25);
                }

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.vietqr.io/v2/generate");

                // Headers
                request.Headers.Add("x-client-id", "ADDING API KEY");
                request.Headers.Add("x-api-key", "ADDING API KEY");

                // Create the payload
                var payload = new
                {
                    accountNo = "777",
                    accountName = "TRAN GIA KHANG",
                    acqId = 970418,
                    amount = (int)amount,
                    addInfo = orderInfo,
                    format = "text",
                    template = "compact"
                };

                // Convert to JSON and set as content
                var jsonContent = JsonConvert.SerializeObject(payload);
                Debug.WriteLine($"Request payload: {jsonContent}");
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                request.Content = content;

                // Send request
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                // Read the response
                var responseContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Response: {responseContent}");

                // Parse the JSON response
                var qrResponse = JsonConvert.DeserializeAnonymousType(responseContent, new
                {
                    code = "",
                    desc = "",
                    data = new { qrCode = "", qrDataURL = "" }
                });

                // Check if we have a valid response
                if (qrResponse?.code != "00" || string.IsNullOrEmpty(qrResponse?.data?.qrDataURL))
                {
                    Debug.WriteLine("Invalid QR response or missing QR data");
                    return null;
                }

                // Extract base64 image data from the data URL
                var base64Data = qrResponse.data.qrDataURL;
                Debug.WriteLine($"Base64 data length: {base64Data.Length}");

                // The data URL format is: data:image/png;base64,[actual base64 data]
                var commaIndex = base64Data.IndexOf(',');
                if (commaIndex > 0)
                {
                    base64Data = base64Data.Substring(commaIndex + 1);
                }

                // Convert base64 to byte array
                var imageBytes = Convert.FromBase64String(base64Data);
                Debug.WriteLine($"Decoded image byte length: {imageBytes.Length}");

                // Create bitmap image from byte array
                var bitmap = new BitmapImage();
                using (var memoryStream = new InMemoryRandomAccessStream())
                {
                    using (var writer = new DataWriter(memoryStream))
                    {
                        writer.WriteBytes(imageBytes);
                        await writer.StoreAsync();
                        await writer.FlushAsync();
                        writer.DetachStream();
                    }

                    memoryStream.Seek(0);
                    await bitmap.SetSourceAsync(memoryStream);
                }

                return bitmap;
            }
            catch (Exception ex)
            {
                // Log exception details for debugging
                Debug.WriteLine($"Error generating QR code: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    Debug.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }

                return null;
            }
        }
    }
}