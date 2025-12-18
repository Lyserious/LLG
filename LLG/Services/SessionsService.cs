using SQLite;
using LLG.Models;

namespace LLG.Services;

public class SessionsService
{
    private SQLiteAsyncConnection? _database;

    async Task Init()
    {
        if (_database is not null) return;

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "LLG.db3");
        _database = new SQLiteAsyncConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);

        await _database.CreateTableAsync<ClimbingSession>();
        await _database.CreateTableAsync<ClimbingRoute>();
    }

    // --- SÉANCES ---

    public async Task<List<ClimbingSession>> GetSessionsAsync()
    {
        await Init();
        return await _database.Table<ClimbingSession>().OrderByDescending(x => x.Date).ToListAsync();
    }

    public async Task SaveSessionWithRoutesAsync(ClimbingSession session, List<ClimbingRoute> routes)
    {
        await Init();

        // 1. Sauvegarde de la séance
        if (session.Id != 0)
            await _database.UpdateAsync(session);
        else
            await _database.InsertAsync(session);

        // 2. Sauvegarde des voies liées
        foreach (var route in routes)
        {
            route.ClimbingSessionId = session.Id;

            if (route.Id != 0)
                await _database.UpdateAsync(route);
            else
                await _database.InsertAsync(route);
        }
    }

    public async Task DeleteSessionAsync(ClimbingSession session)
    {
        await Init();
        // Suppression en cascade : d'abord les voies, puis la séance
        var routes = await GetRoutesForSessionAsync(session.Id);
        foreach (var route in routes)
        {
            await _database.DeleteAsync(route);
        }
        await _database.DeleteAsync(session);
    }

    // --- VOIES (ROUTES) ---

    public async Task<List<ClimbingRoute>> GetRoutesForSessionAsync(int sessionId)
    {
        await Init();
        return await _database.Table<ClimbingRoute>().Where(r => r.ClimbingSessionId == sessionId).ToListAsync();
    }

    // C'est la méthode qui manquait :
    public async Task DeleteRouteAsync(ClimbingRoute route)
    {
        await Init();
        await _database.DeleteAsync(route);
    }
    // --- STATISTIQUES POUR LE DASHBOARD ---

    // 1. Compter les séances de la semaine (Lundi à Dimanche)
    public async Task<int> GetSessionsCountThisWeek()
    {
        await Init();
        // On trouve le lundi de la semaine en cours
        var today = DateTime.Today;
        var diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
        var monday = today.AddDays(-1 * diff);

        // On compte les séances depuis ce lundi
        return await _database.Table<ClimbingSession>()
                              .Where(s => s.Date >= monday)
                              .CountAsync();
    }

    // 2. Calculer le temps total du mois (en heures)
    public async Task<string> GetTotalTimeThisMonth()
    {
        await Init();
        var firstDayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

        var sessions = await _database.Table<ClimbingSession>()
                                      .Where(s => s.Date >= firstDayOfMonth)
                                      .ToListAsync();

        int totalMinutes = sessions.Sum(s => s.DurationMinutes);

        // On convertit joliment (ex: "12h 30")
        int hours = totalMinutes / 60;
        int minutes = totalMinutes % 60;

        return $"{hours}h {minutes:00}";
    }

    // 3. Trouver la meilleure cotation (071009)
    public async Task<string> GetBestGradeThisMonth()
    {
        await Init();

        // LA LISTE COMPLÈTE (Doit être identique au XAML pour marcher)
        var gradesOrder = new List<string>
        {
            "3",
            "4a", "4b", "4c",
            "5a", "5a+", "5b", "5b+", "5c", "5c+",
            "6a", "6a+", "6b", "6b+", "6c", "6c+",
            "7a", "7a+", "7b", "7b+", "7c", "7c+",
            "8a", "8a+", "8b", "8b+", "8c", "8c+",
            "9a", "9a+", "9b", "9b+"
        };

        // On ne prend que les voies RÉUSSIES (IsSuccessful = true)
        var successfulRoutes = await _database.Table<ClimbingRoute>()
                                              .Where(r => r.IsSuccessful)
                                              .ToListAsync();

        string bestGrade = "-";
        int bestGradeIndex = -1;

        foreach (var route in successfulRoutes)
        {
            if (string.IsNullOrEmpty(route.Grade)) continue;

            // On cherche l'index (ex: "6a" est à l'index 10, "7a" est à l'index 16)
            int index = gradesOrder.IndexOf(route.Grade);

            // Si on trouve une cotation plus élevée que la précédente, on la garde
            if (index > bestGradeIndex)
            {
                bestGradeIndex = index;
                bestGrade = route.Grade;
            }
        }

        return bestGrade;
    }
}