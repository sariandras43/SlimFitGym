﻿using Microsoft.Extensions.Logging;
using SlimFitGym_Mobile.Services;
using Microsoft.AspNetCore.Components.WebView.Maui;

namespace SlimFitGym_Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });
            builder.Services.AddScoped<DataService>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://yourbackend.com/api/") });
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
