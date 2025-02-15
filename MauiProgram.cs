﻿using SaYSpin.src.singletons;

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

            AppController mainController = new();


            builder.Services.AddSingleton(provider => mainController);
            builder.Services.AddSingleton<ShowItemInfoDialogService>();
            builder.Services.AddSingleton<BeforeStageActionDialogService>();
            builder.Services.AddSingleton<GameLoggingService>();

            return builder.Build();
        }
    }
}
