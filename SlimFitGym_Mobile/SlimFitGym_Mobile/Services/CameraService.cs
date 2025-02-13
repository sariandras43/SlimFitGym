using Microsoft.AspNetCore.Components;
using System.Diagnostics;

namespace SlimFitGym_Mobile.Services
{
    public class CameraService
    {
        public bool isScanned { get; set; } = true;
        public string Url { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;

        public CameraService() { }

        public async Task<FileResult> InitializeCameraAsync()
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                await MediaPicker.Default.CapturePhotoAsync();
            }
            ErrorMessage = "Kamera nem engedélyezve";
            return null;
        }
    }
}
