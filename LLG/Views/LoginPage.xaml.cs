namespace LLG.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private void OnLoginClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(PseudoEntry.Text))
        {
            DisplayAlert("Erreur", "Il faut un pseudo !", "OK");
            return;
        }

        // 1. On sauvegarde le pseudo (Preuve de connexion)
        Preferences.Set("UserName", PseudoEntry.Text);

        // 2. On change la racine de l'appli pour lancer le vrai AppShell
        Application.Current.MainPage = new AppShell();
    }
}