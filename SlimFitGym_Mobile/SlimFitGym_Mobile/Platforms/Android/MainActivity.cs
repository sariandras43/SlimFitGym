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

            if (CheckSelfPermission(Android.Manifest.Permission.Camera) != Permission.Granted || CheckSelfPermission(Android.Manifest.Permission.RecordAudio) != Permission.Granted)
            {
                RequestPermissions(new string[] { Android.Manifest.Permission.Camera, Android.Manifest.Permission.RecordAudio }, 0);
            }


            Android.Webkit.WebView.SetWebContentsDebuggingEnabled(true);
            Android.Webkit.WebView WebView = new Android.Webkit.WebView(this);
            WebView.Settings.JavaScriptEnabled = true;
            WebView.Settings.MediaPlaybackRequiresUserGesture = false;
            WebView.Settings.AllowFileAccess = true;
            WebView.Settings.AllowContentAccess = true;
        }
    }
}
