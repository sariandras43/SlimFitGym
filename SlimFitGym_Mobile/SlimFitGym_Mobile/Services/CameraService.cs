using Microsoft.AspNetCore.Components;
using System.Diagnostics;
using SlimFitGym_Mobile.Models;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;
using SlimFitGym_Mobile.Components.Pages;

namespace SlimFitGym_Mobile.Services
{
    public class CameraService
    {
        public Grid? qrGrid { get; set; }
        public CameraBarcodeReaderView Camera { get; set; }
        public bool isScanned { get; set; } = true;
        public string Url { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public NavigationManager navigationManager { get; set; }

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
