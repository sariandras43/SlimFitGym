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
        public string Url { get; set; } = "";
        public string ErrorMessage { get; set; } = string.Empty;
        public NavigationManager navigationManager { get; set; }

        public CameraService() { }

        private void Appearing()
        {
            CreateCameraView();
        }

        private void CreateCameraView()
        {
            if (qrGrid != null)
            {
                if (qrGrid.Children.Count > 0)
                {
                    qrGrid.Children.Clear();
                }
                Camera = new CameraBarcodeReaderView();
                Camera.BarcodesDetected += QrCodeDetected;
                qrGrid.Children.Add(Camera);
            }
        }

        public async void QrCodeDetected(object sender, BarcodeDetectionEventArgs e)
        {
            if (e.Results[0].Value == Url)
            {
                Camera.IsDetecting = false;
                EntryModel entry = new EntryModel()
                {
                    AccountId = AccountModel.LoggedInUser.Id,
                    EntryDate = DateTime.Now
                };
                try
                {
                    await DataService.PostEntry(entry);
                    isScanned = true;
                    Task.Delay(1000).ContinueWith(t => navigationManager.NavigateTo("/"));
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                }
                
            }
            else
            {
                ErrorMessage = "Hibás QR kód!";
                return;
            }
        }
    }
}
