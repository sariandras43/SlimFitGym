using QRCodeDecoderLibrary;
using System.Text;
using System.Drawing;
using SkiaSharp;
using ZXing;
using ZXing.QrCode;

namespace SlimFitGym_Mobile.Services
{
    public class CameraService
    {
        public string Url { get; set; }
        public string ErrorMessage { get; set; }
        public bool isScanned { get; set; } = false;

        public async Task InitializeCameraAsync()
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                try
                {
                    var photo = await MediaPicker.Default.CapturePhotoAsync();
                    if (photo == null)
                    {
                        ErrorMessage = "No photo captured.";
                        return;
                    }
                    using (var stream = await photo.OpenReadAsync())
                    {
                        Bitmap bitmap = new Bitmap(stream);

                        QRDecoder decoder = new QRDecoder();

                        byte[][] resultArray = decoder.ImageDecoder(bitmap);

                        if (resultArray != null && resultArray.Length > 0)
                        {
                            string textResult = ByteArrayToStr(resultArray[0]);

                            if (!string.IsNullOrWhiteSpace(textResult))
                            {
                                Url = textResult;
                                isScanned = true;
                            }
                            else
                            {
                                ErrorMessage = "QR code decoded but result is empty.";
                            }
                        }
                        else
                        {
                            ErrorMessage = "QR code not detected.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                }
            }
            else
            {
                ErrorMessage = "Camera is not supported on this device.";
            }
        }

        public static string ByteArrayToStr(byte[] dataArray)
        {
            var decoder = Encoding.UTF8.GetDecoder();
            int charCount = decoder.GetCharCount(dataArray, 0, dataArray.Length);
            char[] charArray = new char[charCount];
            decoder.GetChars(dataArray, 0, dataArray.Length, charArray, 0);
            return new string(charArray);
        }
    }
}


//using var stream = await photo.OpenReadAsync();
//using var skBitmap = SKBitmap.Decode(stream);
//if (skBitmap == null)
//{
//    ErrorMessage = "Failed to load image.";
//    return;
//}
//int width = skBitmap.Width;
//int height = skBitmap.Height;
//byte[] luminanceData = ConvertBitmapToGrayscale(skBitmap);
//var luminanceSource = new RGBLuminanceSource(luminanceData, width, height);
//var binarizer = new HybridBinarizer(luminanceSource);
//var binaryBitmap = new BinaryBitmap(binarizer);
//var hints = new Dictionary<DecodeHintType, object>
//                                        {
//                                            { DecodeHintType.TRY_HARDER, true }
//                                        };
//var reader = new QRCodeReader();
//var result = reader.decode(binaryBitmap, hints);
//if (result != null)
//{
//    Url = result.Text;
//    ErrorMessage = Url;
//}
//else
//{
//    ErrorMessage = "QR code not detected.";
//}

//private byte[] ConvertBitmapToGrayscale(SKBitmap bitmap)
//{
//    int width = bitmap.Width;
//    int height = bitmap.Height;
//    byte[] grayscaleData = new byte[width * height];

//    for (int y = 0; y < height; y++)
//    {
//        for (int x = 0; x < width; x++)
//        {
//            SKColor color = bitmap.GetPixel(x, y);
//            Convert to grayscale using luminosity method
//            byte gray = (byte)((color.Red * 0.3) + (color.Green * 0.59) + (color.Blue * 0.11));
//            grayscaleData[y * width + x] = gray;
//        }
//    }

//    return grayscaleData;
//}