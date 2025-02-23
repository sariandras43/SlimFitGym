using Microsoft.Extensions.DependencyInjection;
using SlimFitGym_Mobile.Models;
using SlimFitGym_Mobile.Services;

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
            AccountModel.LoggedInUser = AuthService.LoadUser();
            InitializeComponent();
            MainPage = new MainPage();
        }


    }
}
