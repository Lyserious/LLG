using LLG;
using LLG.Views;
using Microsoft.Maui.Platform;

namespace LLG;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        // 1. D'ABORD, on choisit quelle page afficher (Login ou Appli ?)
        Page pageDeDemarrage;

        string user = Preferences.Get("UserName", string.Empty);

        if (!string.IsNullOrEmpty(user))
        {
            // Il est connecté -> On prépare l'AppShell
            pageDeDemarrage = new AppShell();
        }
        else
        {
            // Pas connecté -> On prépare le Login
            pageDeDemarrage = new LoginPage();
        }

        // 2. ENSUITE, on crée la fenêtre (window) avec cette page
        // C'est ici qu'on définit la variable 'window' pour que la suite la reconnaisse
        var window = new Window(pageDeDemarrage);

        return window;
    }
}