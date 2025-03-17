using Microsoft.Extensions.Logging;
using SlimFitGym_Mobile.Services;
using Microsoft.AspNetCore.Components.WebView.Maui;
using CommunityToolkit.Maui;
using Microsoft.AspNetCore.Components;
using SlimFitGym_Mobile.Components.Pages;

namespace SlimFitGym_Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });
            builder.Services.AddScoped(sp => new HttpClient());
            builder.Services.AddMauiBlazorWebView();
            builder.Services.ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler<BlazorWebView, BlazorWebViewHandler>();
            });

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif


            return builder.Build();
        }
    }
}
