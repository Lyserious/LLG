using System.Collections.ObjectModel;
using LLG.Models;
using LLG.Services;

namespace LLG.Views;


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

    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        
        if (query.ContainsKey("Session"))
        {
            CurrentSession = query["Session"] as ClimbingSession;
            OnPropertyChanged(nameof(CurrentSession));
            LoadExistingRoutes();
        }

        
        if (query.ContainsKey("ReturnedRoute"))
        {
            var route = query["ReturnedRoute"] as ClimbingRoute;

            
            var existing = TemporaryRoutes.FirstOrDefault(r =>
                (r.Id != 0 && r.Id == route.Id) || r == route);

            if (existing != null)
            {
                
                int index = TemporaryRoutes.IndexOf(existing);
                TemporaryRoutes[index] = route;
            }
            else
            {
                
                TemporaryRoutes.Add(route);
            }
        }

        
        query.Clear();
    }

    private async void LoadExistingRoutes()
    {
        if (CurrentSession != null && CurrentSession.Id != 0)
        {
            
            if (TemporaryRoutes.Count == 0)
            {
                var routes = await _service.GetRoutesForSessionAsync(CurrentSession.Id);
                foreach (var route in routes) TemporaryRoutes.Add(route);
            }
        }
    }

    

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