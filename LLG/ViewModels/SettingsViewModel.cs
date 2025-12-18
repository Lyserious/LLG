using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LLG.Views;

namespace LLG.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    // --- VARIABLES ---
    [ObservableProperty]
    private string userName;

    [ObservableProperty]
    private string userEmail;

    [ObservableProperty]
    private string userPassword; // Celui qu'on tape dans le champ

    [ObservableProperty]
    private string userWeight;

    [ObservableProperty]
    private string userHeight;

    // --- NOUVEAU : GESTION DU MODE ÉDITION ---

    // Par défaut, IsEditing est faux (Lecture seule)
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotEditing))] // Dit à la vue de mettre à jour l'inverse aussi
    private bool isEditing = false;

    // Une petite astuce pour avoir l'inverse (utile pour le XAML)
    public bool IsNotEditing => !IsEditing;

    public SettingsViewModel()
    {
        LoadData();
    }

    private void LoadData()
    {
        UserName = Preferences.Default.Get("UserName", string.Empty);
        UserEmail = Preferences.Default.Get("UserEmail", string.Empty);

        // SECURITÉ : On ne charge PAS le mot de passe dans le champ visible.
        // On le laisse vide. Comme ça, pas de risque de le modifier par erreur.
        UserPassword = string.Empty;

        UserWeight = Preferences.Default.Get("UserWeight", string.Empty);
        UserHeight = Preferences.Default.Get("UserHeight", string.Empty);
    }

    // Commande pour activer/désactiver le mode modification
    [RelayCommand]
    private void ToggleEditMode()
    {
        IsEditing = !IsEditing;

        // Si on annule (qu'on repasse en mode lecture), on recharge les vieilles données
        if (!IsEditing)
        {
            LoadData();
        }
    }

    [RelayCommand]
    private async Task SaveData()
    {
        Preferences.Default.Set("UserName", UserName);
        Preferences.Default.Set("UserEmail", UserEmail);
        Preferences.Default.Set("UserWeight", UserWeight);
        Preferences.Default.Set("UserHeight", UserHeight);

        // LOGIQUE INTELLIGENTE POUR LE MDP
        // On ne le change QUE si l'utilisateur a écrit quelque chose dedans.
        if (!string.IsNullOrWhiteSpace(UserPassword))
        {
            Preferences.Default.Set("UserPassword", UserPassword);
        }

        await Shell.Current.DisplayAlert("Succès", "Profil mis à jour !", "OK");

        // On revérrouille le formulaire après la sauvegarde
        IsEditing = false;

        // On vide le champ mot de passe visuellement
        UserPassword = string.Empty;

    }
    [RelayCommand]
    private void Logout()
    {
        // 1. On efface la preuve de connexion
        // (Tu peux choisir d'effacer aussi le Poids/Taille ou de les garder)
        Preferences.Remove("UserName");

        // 2. On renvoie l'utilisateur sur la page de Login
        Application.Current.MainPage = new LoginPage();
    }
}