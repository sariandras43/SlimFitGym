using Microsoft.AspNetCore.Components;
using System.Diagnostics;
using SlimFitGym_Mobile.Models;
using SlimFitGym_Mobile.Components.Pages;

namespace SlimFitGym_Mobile.Services
{
    public class CameraService
    {
        public  bool isScanned { get; set; } = false;
        public  string Url { get; set; }
        public  string ErrorMessage { get; set; } = string.Empty;
        private readonly NavigationManager _navigationManager;

        public CameraService(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }
        public  async Task InitializeCameraAsync()
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                var photo = await MediaPicker.Default.CapturePhotoAsync();
                // qr code implementation
            }
            else
            {
                ErrorMessage = "Kamera nem engedélyezve";
                return;
            }
        }
    }
}
