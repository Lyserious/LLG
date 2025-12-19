using SQLite;

namespace LLG.Models;

public class ClimbingSession
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;
    public int DurationMinutes { get; set; }

    public string? LocationName { get; set; } 
    public bool IsIndoors { get; set; }

    public string? Name { get; set; }  

    [Ignore] 
    public string DisplayTitle
    {
        get
        {
            // --- 1. Gestion des cas où il n'y a pas de nom ---
            if (string.IsNullOrWhiteSpace(Name))
            {
                if (string.IsNullOrWhiteSpace(LocationName))
                {
                    return "Séance libre";
                }
                // Si pas de nom mais qu'il y a un lieu
                return $"Séance à {LocationName ?? "Inconnu"}";
            }

            // --- 2. Gestion quand on a un nom ---

            string cleanName = Name.Trim();
            string prefix = "";

            // Si le nom ne commence PAS par "Séance" (insensible à la casse), on prépare le préfixe
            if (!cleanName.StartsWith("Séance", StringComparison.OrdinalIgnoreCase))
            {
                prefix = "Séance ";
            }

            // 
            if(string.IsNullOrWhiteSpace(LocationName))
                return $"{prefix}{cleanName} ";

            // On retourne le résultat final composé
            return $"{prefix}{cleanName} à {LocationName ?? "Inconnu"}";
        }
    }


    public int InitialMood { get; set; }
    public int FinalMood { get; set; }
    public string? Notes { get; set; } // Note le ?
}