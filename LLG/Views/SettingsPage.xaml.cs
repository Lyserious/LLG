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

    // TA FONCTION MAGIQUE : Ouvre le menu quand on quitte
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Shell.Current.FlyoutIsPresented = true;
    }
}