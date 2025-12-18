using SQLite;

namespace LLG.Models;

public class ClimbingRoute
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int ClimbingSessionId { get; set; } 

    // Ajoute les '?' ici
    public string? Name { get; set; } 
    public string? Grade { get; set; } 
    public string? Type { get; set; } = "Voie"; 
    public string? Style { get; set; } 
    
    public bool IsSuccessful { get; set; }
    public int Attempts { get; set; } 
    public string? Notes { get; set; }
}