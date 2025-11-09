using System.Text.Json;

public class TennisMatchRepository : ITennisMatchRepository
{
    private readonly string _filePath = "score.json";
    public async Task<(string player1, string player2)> GetScoreAsync()
    {
        if (!File.Exists(_filePath))
        {
            return ("Love", "Love");
        }

        var json = await File.ReadAllTextAsync(_filePath);
        var score = JsonSerializer.Deserialize<ScoreDto>(json);
        return (score.Player1, score.Player2);
    }
}