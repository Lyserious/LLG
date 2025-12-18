using LLG.Models;

namespace LLG.Views;

[QueryProperty(nameof(RouteToEdit), "Route")]
public partial class RouteDetailPage : ContentPage
{
    ClimbingRoute? _routeToEdit;

    public ClimbingRoute RouteToEdit
    {
        get => _routeToEdit;
        set
        {
            _routeToEdit = value;
            if (_routeToEdit != null)
                BindingContext = _routeToEdit;
        }
    }

    public RouteDetailPage()
    {
        InitializeComponent();
        BindingContext = new ClimbingRoute { Name = "", Grade = "6a", Type = "Voie", Attempts = 1, Style = "En tête" };
    }

    private async void OnValidateClicked(object sender, EventArgs e)
    {
        var route = (ClimbingRoute)BindingContext;

        // CHANGEMENT ICI : On prépare le colis de retour
        var navigationParameter = new Dictionary<string, object>
        {
            { "ReturnedRoute", route }
        };

        // On retourne en arrière ("..") AVEC le colis
        await Shell.Current.GoToAsync("..", navigationParameter);
    }
}