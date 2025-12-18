using System.Collections.ObjectModel;
using LLG.Models;
using LLG.Services;

namespace LLG.Views;

// On retire [QueryProperty] et on ajoute l'interface IQueryAttributable
public partial class SessionDetailPage : ContentPage, IQueryAttributable
{
    private readonly SessionsService _service;

    public ObservableCollection<ClimbingRoute> TemporaryRoutes { get; set; } = new();
    public ClimbingSession CurrentSession { get; set; }

    public SessionDetailPage(SessionsService service)
    {
        InitializeComponent();
        _service = service;

        // Setup initial
        CurrentSession = new ClimbingSession { Date = DateTime.Now, DurationMinutes = 120 };
        BindingContext = this;
    }

    // CETTE MÉTHODE MAGIQUE S'ACTIVE QUAND ON ARRIVE SUR LA PAGE
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        // CAS 1 : On arrive depuis la liste des Séances (On charge la séance)
        if (query.ContainsKey("Session"))
        {
            CurrentSession = query["Session"] as ClimbingSession;
            OnPropertyChanged(nameof(CurrentSession));
            LoadExistingRoutes();
        }

        // CAS 2 : On revient de la page "Ajout Voie" (On ajoute la voie)
        if (query.ContainsKey("ReturnedRoute"))
        {
            var route = query["ReturnedRoute"] as ClimbingRoute;

            // On vérifie si c'est une modif ou un ajout
            var existing = TemporaryRoutes.FirstOrDefault(r =>
                (r.Id != 0 && r.Id == route.Id) || r == route);

            if (existing != null)
            {
                // Modification
                int index = TemporaryRoutes.IndexOf(existing);
                TemporaryRoutes[index] = route;
            }
            else
            {
                // Ajout
                TemporaryRoutes.Add(route);
            }
        }

        // Petit nettoyage pour éviter de recharger 2 fois les données si on reclique
        query.Clear();
    }

    private async void LoadExistingRoutes()
    {
        if (CurrentSession != null && CurrentSession.Id != 0)
        {
            // On ne recharge que si la liste est vide (pour ne pas écraser les ajouts récents)
            if (TemporaryRoutes.Count == 0)
            {
                var routes = await _service.GetRoutesForSessionAsync(CurrentSession.Id);
                foreach (var route in routes) TemporaryRoutes.Add(route);
            }
        }
    }

    // --- LE RESTE NE CHANGE PAS ---

    private async void OnAddRouteClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(RouteDetailPage));
    }

    private async void OnRouteSelected(object sender, TappedEventArgs e)
    {
        var selectedRoute = e.Parameter as ClimbingRoute;
        if (selectedRoute == null) return;

        var navParam = new Dictionary<string, object> { { "Route", selectedRoute } };
        await Shell.Current.GoToAsync(nameof(RouteDetailPage), navParam);
    }

    private async void OnDeleteRouteClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var routeToDelete = button?.CommandParameter as ClimbingRoute;
        if (routeToDelete == null) return;

        bool confirm = await DisplayAlert("Supprimer", "Retirer cette voie ?", "Oui", "Non");
        if (!confirm) return;

        if (routeToDelete.Id != 0) await _service.DeleteRouteAsync(routeToDelete);
        TemporaryRoutes.Remove(routeToDelete);
    }

    private async void OnSaveSessionClicked(object sender, EventArgs e)
    {
        if (CurrentSession == null) return;
        await _service.SaveSessionWithRoutesAsync(CurrentSession, TemporaryRoutes.ToList());
        await Shell.Current.GoToAsync("..");
    }

    private async void OnDeleteSessionClicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Attention", "Supprimer la séance ?", "Oui", "Non");
        if (!answer) return;

        if (CurrentSession != null && CurrentSession.Id != 0)
            await _service.DeleteSessionAsync(CurrentSession);

        await Shell.Current.GoToAsync("..");
    }
}