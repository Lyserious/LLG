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
        
        SessionsList.ItemsSource = await _service.GetSessionsAsync();
    }

    
    private async void OnSessionSelected(object sender, SelectionChangedEventArgs e)
    {
        
        var selectedSession = e.CurrentSelection.FirstOrDefault() as ClimbingSession;

        
        if (selectedSession == null) return;

        
        
        var navigationParameter = new Dictionary<string, object>
        {
            { "Session", selectedSession }
        };

        
        await Shell.Current.GoToAsync(nameof(SessionDetailPage), navigationParameter);

        
        ((CollectionView)sender).SelectedItem = null;
    }

    private async void OnAddSessionClicked(object sender, EventArgs e)
    {
        
        await Shell.Current.GoToAsync(nameof(SessionDetailPage));
    }
}