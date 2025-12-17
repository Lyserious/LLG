using System;
using System.Collections.Generic;
using System.Text;

namespace LLG.Models;

public class ClimbingRoute
{
    public int Id { get; set; } // Identifiant unique

    public string Grade { get; set; } // Cotation (ex: "6a", "7b+")

    // Style de grimpe : "Voie" ou "Bloc"
    public string Style { get; set; }

    // Résultat : "OnSight" (A vue), "Flash", "Redpoint" (Après travail), "Failed" (Echec)
    public string Status { get; set; }

    public string Note { get; set; } // Commentaire optionnel sur la voie

    // Pour l'affichage facile dans la liste (ex: "6a - Flash")
    public string DisplayInfo => $"{Grade} - {Status}";
}