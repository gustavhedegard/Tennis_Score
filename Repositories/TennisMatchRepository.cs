using System.Text.Json;

public class TennisMatchRepository : ITennisMatchRepository
{
    private readonly string _filePath = "score.json";
    public async Task<MatchInfoDto> GetScoreAsync()
    {
        if (!File.Exists(_filePath))
        {
            return new MatchInfoDto
            {
                PlayerAScore = "Love",
                PlayerBScore = "Love",
                Advantage = ""

            };
        }

        var json = await File.ReadAllTextAsync(_filePath);
        var matchInfo = JsonSerializer.Deserialize<MatchInfoDto>(json);

        if (matchInfo == null)
        {
            return new MatchInfoDto
            {
                PlayerAScore = "Love",
                PlayerBScore = "Love",
                Advantage = ""

            };
        }

        return matchInfo;
    }

    public async Task AssignScoreAsync(MatchInfoDto matchInfo)
    {
        var matchInfojson = JsonSerializer.Serialize(matchInfo, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        await File.WriteAllTextAsync(_filePath, matchInfojson);

    }
    
    public async Task NewGameAsync()
    {
        if(File.Exists(_filePath))
        {
            File.Delete(_filePath);
        }
    }
}