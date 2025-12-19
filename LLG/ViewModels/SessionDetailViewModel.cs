using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LLG.Models;
using LLG.Services; 

namespace LLG.ViewModels;

public partial class SessionDetailViewModel : ObservableObject
{
    private readonly SessionsService _service; // Le lien vers la DB

    // --- PROPRIÉTÉS DU FORMULAIRE ---

    [ObservableProperty]
    private DateTime date = DateTime.Now;

    [ObservableProperty]
    private int durationMinutes = 120;

    // C'est lui le "Titre" que tu cherchais !
    [ObservableProperty]
    private string? name;

    [ObservableProperty]
    private string? locationName;

    [ObservableProperty]
    private bool isIndoors = true;

    [ObservableProperty]
    private int initialMood = 5;

    [ObservableProperty]
    private int finalMood = 5;

    [ObservableProperty]
    private string? notes;

    // Constructeur : On injecte le service ici
    public SessionDetailViewModel(SessionsService service)
    {
        _service = service;
    }

    [RelayCommand]
    private async Task Cancel()
    {
        await Shell.Current.GoToAsync("..");
    }

    // --- LA SAUVEGARDE RÉELLE ---
    [RelayCommand]
    private async Task Save()
    {
        // 1. On crée l'objet Session prêt à être enregistré
        var session = new ClimbingSession
        {
            Date = Date,
            DurationMinutes = DurationMinutes,

            // On fait le lien entre tes champs et la base de données :
            Name = Name,                // Le Titre
            LocationName = LocationName,       // Le Lieu (Lieu inconnu si vide)
            IsIndoors = IsIndoors,

            InitialMood = InitialMood,
            FinalMood = FinalMood,
            Notes = Notes
        };

        // 2. On appelle le service pour sauvegarder (avec une liste de voies vide pour l'instant)
        await _service.SaveSessionWithRoutesAsync(session, new List<ClimbingRoute>());

        // 3. On revient en arrière
        await Shell.Current.GoToAsync("..");
    }
}