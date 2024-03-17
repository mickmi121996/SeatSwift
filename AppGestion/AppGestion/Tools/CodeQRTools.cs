using QRCoder;
using System;
using System.IO;

namespace AppGestion.Tools
{
    public class CodeQRTools
    {
        /// <summary>
        /// Generate a QR code from a string
        /// </summary>
        /// <param name="data">The data to encode in the QR code</param>
        /// <returns>The base64 string of the QR code</returns>
        public static string GenerateQRCodeBase64(string data)
        {
            using (var qrGenerator = new QRCodeGenerator())
            {
                using (var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q))
                {
                    using (var qrCode = new QRCode(qrCodeData))
                    {
                        using (var qrCodeImage = qrCode.GetGraphic(20))
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                qrCodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                var base64String = Convert.ToBase64String(ms.ToArray());
                                return "data:image/png;base64," + base64String;
                            }
                        }
                    }
                }
            }
        }
    }
}
