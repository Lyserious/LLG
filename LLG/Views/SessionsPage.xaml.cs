using LLG.Models;
using LLG.Services;
using LLG.Views;


namespace LLG.Views;

public partial class SessionsPage : ContentPage
{
    private readonly SessionsService _service;

    public SessionsPage(SessionsService service)
    {
        InitializeComponent();
        _service = service;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // On recharge la liste à chaque fois qu'on revient sur cette page
        SessionsList.ItemsSource = await _service.GetSessionsAsync();
    }

    // C'est ICI que le transfert se fait
    private async void OnSessionSelected(object sender, SelectionChangedEventArgs e)
    {
        // 1. On vérifie ce qu'on a touché
        var selectedSession = e.CurrentSelection.FirstOrDefault() as ClimbingSession;

        // Si c'est vide (bug de clic), on arrête tout
        if (selectedSession == null) return;

        // 2. On prépare le colis
        // La clé "Session" est le mot de passe pour que l'autre page comprenne
        var navigationParameter = new Dictionary<string, object>
        {
            { "Session", selectedSession }
        };

        // 3. On navigue en envoyant le colis
        await Shell.Current.GoToAsync(nameof(SessionDetailPage), navigationParameter);

        // 4. On "dé-sélectionne" la ligne pour qu'elle ne reste pas grise/orange
        ((CollectionView)sender).SelectedItem = null;
    }

    private async void OnAddSessionClicked(object sender, EventArgs e)
    {
        // Ici on n'envoie rien, donc l'autre page créera une séance vide
        await Shell.Current.GoToAsync(nameof(SessionDetailPage));
    }
}