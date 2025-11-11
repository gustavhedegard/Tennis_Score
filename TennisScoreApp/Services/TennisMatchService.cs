public class TennisMatchService : ITennisMatchService
{
    private readonly ITennisMatchRepository _tennisMatchRepository;

    public TennisMatchService(ITennisMatchRepository tennisMatchRepository)
    {
        _tennisMatchRepository = tennisMatchRepository;
    }

    public async Task AssignPointAsync(AssignPointDto request)
    {
        var matchInfo = await GetMatchInfoAsync();

        if (matchInfo.Winner != null)
            return;

        bool isPlayerA = request.Player == Player.A;
        Score currentPlayerScore = isPlayerA ? matchInfo.PlayerAScore : matchInfo.PlayerBScore;
        Score opponentScore = isPlayerA ? matchInfo.PlayerBScore : matchInfo.PlayerAScore;

        if (currentPlayerScore == Score.Forty && !IsDeuce(currentPlayerScore, opponentScore))
        {
            matchInfo.Winner = request.Player;
            await SaveScoreAsync(matchInfo);
            return;
        }

        if (IsDeuce(matchInfo.PlayerAScore, matchInfo.PlayerBScore))
        {
            HandleDeuce(matchInfo, request.Player);
            await SaveScoreAsync(matchInfo);
            return;
        }

        if (currentPlayerScore < Score.Forty)
        {
            if (isPlayerA)
            {
                matchInfo.PlayerAScore++;
            }
            else
            {
                matchInfo.PlayerBScore++;
            }

            if (IsDeuce(matchInfo.PlayerAScore, matchInfo.PlayerBScore))
            {
                matchInfo.Deuce = true;
            }

            await SaveScoreAsync(matchInfo);
            return;
        }
    }

    private static bool IsDeuce(Score firstScore, Score secondScore)
    {
        if (firstScore == Score.Forty && secondScore == Score.Forty)
        {
            return true;
        }

        return false;
    }

    private static void HandleDeuce(MatchInfoDto matchInfo, Player player)
    {
        if (matchInfo.Advantage == player)
        {
            matchInfo.Winner = player;
            matchInfo.Advantage = null;
            matchInfo.Deuce = false;
        }
        else if (matchInfo.Advantage == null)
        {
            matchInfo.Advantage = player;
            matchInfo.Deuce = false;
        }
        else
        {
            matchInfo.Advantage = null;
            matchInfo.Deuce = true;
        }
    }
    
    public async Task<MatchInfoDto> GetMatchInfoAsync()
    {
        return await _tennisMatchRepository.GetMatchInfoAsync();
    }

    public async Task ResetMatchAsync()
    {
        await _tennisMatchRepository.ResetMatchAsync();
    }
    
    private async Task SaveScoreAsync(MatchInfoDto matchInfo)
    {
        await _tennisMatchRepository.AssignScoreAsync(matchInfo);
    }
    
}