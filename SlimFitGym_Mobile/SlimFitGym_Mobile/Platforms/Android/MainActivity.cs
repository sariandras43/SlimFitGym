using Android.App;
using Android.Content.PM;
using Android.OS;

namespace SlimFitGym_Mobile
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);

            if (CheckSelfPermission(Android.Manifest.Permission.Camera) != Permission.Granted)
            {
                RequestPermissions(new string[] { Android.Manifest.Permission.Camera }, 0);
            }

            Android.Webkit.WebView.SetWebContentsDebuggingEnabled(true);
        }
    }
}
