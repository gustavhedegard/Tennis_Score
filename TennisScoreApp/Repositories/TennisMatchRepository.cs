using System.Text.Json;
using System.Text.Json.Serialization;

public class TennisMatchRepository : ITennisMatchRepository
{
    private readonly string _filePath = "matchInfo.json";
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };
    
    public async Task<MatchInfoDto> GetMatchInfoAsync()
    {
        if (!File.Exists(_filePath))
        {
            return CreateNewMatch();
        }

        var json = await File.ReadAllTextAsync(_filePath);
        var matchInfo = JsonSerializer.Deserialize<MatchInfoDto>(json, JsonOptions);

        return matchInfo ?? CreateNewMatch();
    }

    public async Task ResetMatchAsync()
    {
        var newMatch = CreateNewMatch();
        await AssignScoreAsync(newMatch);
    }
    
    public async Task AssignScoreAsync(MatchInfoDto matchInfo)
    {
        var matchInfojson = JsonSerializer.Serialize(matchInfo, JsonOptions);
        await File.WriteAllTextAsync(_filePath, matchInfojson);
    }

    private static MatchInfoDto CreateNewMatch()
    {
        return new MatchInfoDto
        {
            PlayerAScore = Score.Love,
            PlayerBScore = Score.Love,
            Advantage = null,
            Deuce = false,
            Winner = null
        };
    }
}