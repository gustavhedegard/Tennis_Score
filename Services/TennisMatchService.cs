using System.Text.RegularExpressions;

public class TennisMatchService : ITennisMatchService
{
    private readonly ITennisMatchRepository _tennisMatchRepository;

    public TennisMatchService(ITennisMatchRepository tennisMatchRepository)
    {
        _tennisMatchRepository = tennisMatchRepository;
    }

    public async Task<MatchInfoDto> GetMatchInfoAsync()
    {
        return await _tennisMatchRepository.GetMatchInfoAsync();
    }
    public async Task AssignPointAsync(AssignPointDto request)
    {
        var matchInfo = await _tennisMatchRepository.GetMatchInfoAsync();

        if (matchInfo.Winner != null)
            return;

        bool isPlayerA = request.Player == Player.A;
        Score currentPlayerScore = isPlayerA ? matchInfo.PlayerAScore : matchInfo.PlayerBScore;
        Score opponentScore = isPlayerA ? matchInfo.PlayerBScore : matchInfo.PlayerAScore;

        if (currentPlayerScore == Score.Forty && opponentScore != Score.Forty)
        {
            matchInfo.Winner = request.Player;
            await _tennisMatchRepository.AssignScoreAsync(matchInfo);
            return;
        }
        
        if (matchInfo.PlayerAScore == Score.Forty && matchInfo.PlayerBScore == Score.Forty)
        {
            HandleDeuce(matchInfo, request.Player);
            await _tennisMatchRepository.AssignScoreAsync(matchInfo);
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

            await _tennisMatchRepository.AssignScoreAsync(matchInfo);
            return;
        }
    }

    public async Task ResetMatchAsync()
    {
        await _tennisMatchRepository.ResetMatchAsync();
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
        } else
        {
            matchInfo.Advantage = null;
            matchInfo.Deuce = true;
        }
    }
}