﻿
using SaYSpin.src.singletons;

namespace SaYSpin
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

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
#endif

            src.Logger.Init();
            MainGameController gameController = new();
            builder.Services.AddSingleton<MainGameController>();
            return builder.Build();
        }
    }
}
