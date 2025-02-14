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
            AccountModel.LoggedInUser = AuthService.LoadUser();
            DeviceTheme = Application.Current.RequestedTheme;
            InitializeComponent();

            MainPage = new MainPage();
        }

    }
}
