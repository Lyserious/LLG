using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace LLG.Models;

public class ClimbingSession
{
    public int Id { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;

    public string GymName { get; set; } // Nom de la salle ou du site

    public TimeSpan Duration { get; set; } // Durée de la séance

    public string Notes { get; set; } // Ressenti global (ex: "Super forme", "Fatigué")

    // Liste des voies faites durant cette séance
    // On utilise ObservableCollection pour que l'interface se mette à jour automatiquement si on ajoute une voie
    public ObservableCollection<ClimbingRoute> Routes { get; set; } = new ObservableCollection<ClimbingRoute>();

    // Helper pour afficher un résumé (ex: "12/12/2025 - Arkose Nation")
    public string DisplayTitle => $"{Date:d} - {GymName}";
}