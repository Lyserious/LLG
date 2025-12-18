using LLG.Views;


namespace LLG;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Enregistrement des routes
        Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));

        // Enregistrement des routes de navigation
        Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
        Routing.RegisterRoute(nameof(SessionDetailPage), typeof(SessionDetailPage));
        Routing.RegisterRoute(nameof(Views.SessionDetailPage), typeof(Views.SessionDetailPage));
        Routing.RegisterRoute(nameof(Views.RouteDetailPage), typeof(Views.RouteDetailPage));
    }

    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        // 1. On ferme le menu latéral (sinon il reste ouvert)
        Shell.Current.FlyoutIsPresented = false;

        // 2. On navigue vers la page Settings
        await Shell.Current.GoToAsync(nameof(SettingsPage));
    }
}