using System;
using LLG.ViewModels;

namespace LLG.Views;

public partial class SettingsPage : ContentPage
{
    // On injecte le ViewModel ici
    public SettingsPage(SettingsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    // Bouton retour défini dans le XAML : OnBackButtonClicked
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        try
        {
            // Tente de revenir d'un niveau dans la stack Shell
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception)
        {
            // En fallback, essayer PopAsync si GoToAsync échoue
            try
            {
                await Navigation.PopAsync();
            }
            catch
            {
                // Pas d'action supplémentaire — on ignore l'erreur pour éviter crash
            }
        }
    }

    // TA FONCTION MAGIQUE : Ouvre le menu quand on quitte
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Shell.Current.FlyoutIsPresented = true;

#if WINDOWS || MACCATALYST
    Shell.Current.FlyoutBehavior = FlyoutBehavior.Locked;
#else
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
#endif
    }
}