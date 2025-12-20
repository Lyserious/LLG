using System;
using LLG.Views;


namespace LLG;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Définit le comportement du flyout par plateforme (équivalent de OnPlatform en XAML)
        if (OperatingSystem.IsWindows() || OperatingSystem.IsMacCatalyst())
        {
            // WinUI et MacCatalyst : menu verrouillé
            FlyoutBehavior = FlyoutBehavior.Locked;
        }
        else if (OperatingSystem.IsAndroid() || OperatingSystem.IsIOS())
        {
            // Android et iOS : flyout activé
            FlyoutBehavior = FlyoutBehavior.Flyout;
        }
        else
        {
            // Valeur par défaut
            FlyoutBehavior = FlyoutBehavior.Flyout;
        }

        // Enregistrement des routes
        Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
        Routing.RegisterRoute(nameof(SessionDetailPage), typeof(SessionDetailPage));
        Routing.RegisterRoute(nameof(Views.SessionDetailPage), typeof(Views.SessionDetailPage));
        Routing.RegisterRoute(nameof(Views.RouteDetailPage), typeof(Views.RouteDetailPage));
    }

    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        // 1. On ferme le menu latéral (sinon il reste ouvert)
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
        Shell.Current.FlyoutIsPresented = false;

        // 2. On navigue vers la page Settings
        await Shell.Current.GoToAsync(nameof(SettingsPage));
    }
}