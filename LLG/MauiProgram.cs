using LLG.ViewModels;
using LLG.Views;
using LLG.Services;
using Microsoft.Extensions.Logging;

namespace LLG
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            // 1. SERVICES
            builder.Services.AddSingleton<SessionsService>();

            //// 2. VIEWMODELS
            //builder.Services.AddTransient<DashboardViewModel>();
            builder.Services.AddTransient<SettingsViewModel>(); // Ajouté ici pour permettre l'injection

            // 3. VUES (PAGES)
            builder.Services.AddTransient<SessionsPage>();
            builder.Services.AddTransient<SessionDetailPage>();
            builder.Services.AddTransient<DashboardPage>();
            builder.Services.AddTransient<RouteDetailPage>();
            builder.Services.AddTransient<SettingsPage>();
            builder.Services.AddTransient<StatsPage>();
            builder.Services.AddTransient<ProjectsPage>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
