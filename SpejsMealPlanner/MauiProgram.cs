using Microsoft.Extensions.Logging;
using SpejsMealPlanner.Data;
using SpejsMealPlanner.ViewModels;

namespace SpejsMealPlanner;

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
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        
        builder.Services.AddSingleton<Database>();

        builder.Services.AddTransient<MainPageViewModel>();
        builder.Services.AddTransient<MealsManagerViewModel>();
        builder.Services.AddTransient<HistoryPageViewModel>();
        builder.Services.AddTransient<SastojciViewModel>();
        builder.Services.AddTransient<AddMealViewModel>();
        builder.Services.AddTransient<ViewModels.RegisterViewModel>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<ViewModels.LoginViewModel>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<MealsManagerPage>();
        builder.Services.AddTransient<HistoryPage>();
        builder.Services.AddTransient<SastojciPage>();
        builder.Services.AddTransient<AddMealPage>();
        builder.Services.AddTransient<SelectMealPage>();

        return builder.Build();
    }
}