using Microsoft.Extensions.DependencyInjection;
using SlimFitGym_Mobile.Models;
using SlimFitGym_Mobile.Services;
using System.Text.Json;

namespace SlimFitGym_Mobile
{
    public partial class App : Application
    {
        public AppTheme DeviceTheme { get; set; }
        public App()
        {
            var internetConnection = Connectivity.Current.NetworkAccess;
            if (internetConnection != NetworkAccess.Internet)
            {
                MainPage = new MainPage();
                Application.Current.Dispatcher.Dispatch(async () =>
                {
                    await MainPage.DisplayAlert("No Internet Connection", "Az alkalmazás használatához internet kapcsolat szükséges!", "OK");
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                });
                return;
            }

            DeviceTheme = Application.Current.RequestedTheme;
            OnStartAsync();
            InitializeComponent();
            MainPage = new MainPage();
        }

        private async void OnStartAsync()
        {
            await AuthService.LoadUser();
        }
    }
}
