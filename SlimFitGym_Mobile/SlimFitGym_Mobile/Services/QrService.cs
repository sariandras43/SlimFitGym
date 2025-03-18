using System.Runtime.InteropServices;
using System.Text.Json;
using SkiaSharp;
using SlimFitGym_Mobile.Models;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.QrCode.Internal;

namespace SlimFitGym_Mobile.Services
{
    public static class QrService
    {
        public static string GenerateQrCode()
        {
            var writer = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = 300,
                    Width = 300,
                    Margin = 1,
                    ErrorCorrection = ErrorCorrectionLevel.H
                }
            };
            var json = JsonSerializer.Serialize(new
            {
                id = AccountModel.LoggedInUser.Id,
                name = AccountModel.LoggedInUser.Name
            });
            var pixelData = writer.Write(json);
            using (var bitmap = new SKBitmap(pixelData.Width, pixelData.Height, SKColorType.Bgra8888, SKAlphaType.Premul))
            {
                Marshal.Copy(pixelData.Pixels, 0, bitmap.GetPixels(), pixelData.Pixels.Length);
                using (var image = SKImage.FromBitmap(bitmap))
                using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                {
                    var base64 = Convert.ToBase64String(data.ToArray());
                    return $"data:image/png;base64,{base64}";
                }
            }
        }
    }
}
