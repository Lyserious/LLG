using CommunityToolkit.Mvvm.ComponentModel;
using LLG.Services;

namespace LLG.ViewModels;

public partial class DashboardViewModel : ObservableObject
{
    private readonly SessionsService _service;

    // --- AJOUT ---
    [ObservableProperty]
    private string userName = "Grimpeur"; // Valeur par défaut

    [ObservableProperty]
    private string sessionsThisWeek = "-";

    [ObservableProperty]
    private string climbingTimeMonth = "-";

    [ObservableProperty]
    private string maxGrade = "-";

    public DashboardViewModel(SessionsService service)
    {
        _service = service;
    }

    public async Task LoadData()
    {
       
        // Si l'utilisateur n'a pas mis de nom, on affiche "Grimpeur"
        var storedName = Preferences.Default.Get("UserName", string.Empty);
        if (!string.IsNullOrEmpty(storedName))
        {
            UserName = storedName;
        }

        
        var count = await _service.GetSessionsCountThisWeek();
        SessionsThisWeek = count.ToString("00");

        ClimbingTimeMonth = await _service.GetTotalTimeThisMonth();

        var bestGrade = await _service.GetBestGradeThisMonth();
        MaxGrade = bestGrade;
    }
}