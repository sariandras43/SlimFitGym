using SlimFitGym_Mobile.Models;
using SlimFitGym_Mobile.Services;

namespace SlimFitGym_Mobile
{
    public partial class App : Application
    {
        public App()
        {
            AccountModel.LoggedInUser = AuthService.LoadUser();
            InitializeComponent();

            MainPage = new MainPage();
        }

    }
}
