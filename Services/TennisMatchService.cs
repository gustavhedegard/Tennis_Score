using System.Text.RegularExpressions;

public class TennisMatchService : ITennisMatchService
{
    private readonly ITennisMatchRepository _tennisMatchRepository;
    private readonly string[] Scores = { "Love", "Fifteen", "Thirty", "Forty" };

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

        bool isPlayerA = request.Player == "A";
        string currentPlayerScore = isPlayerA ? matchInfo.PlayerAScore : matchInfo.PlayerBScore;
        string opponentScore = isPlayerA ? matchInfo.PlayerBScore : matchInfo.PlayerAScore;

        if (matchInfo.PlayerAScore == "Forty" && matchInfo.PlayerBScore == "Forty")
        {
            // player already has advantege and wins
            if (matchInfo.Advantage == request.Player)
            {
                matchInfo.Winner = request.Player;
            } // deuce and player gets advantage
            else if (string.IsNullOrEmpty(matchInfo.Advantage))
            {
                matchInfo.Advantage = request.Player;
                matchInfo.Deuce = false;
            }
            else // other player has advantage --> deuce
            {
                matchInfo.Advantage = "";
                matchInfo.Deuce = true;
            }

            await _tennisMatchRepository.AssignScoreAsync(matchInfo);
            return;
        }

        int currentIndex = Array.IndexOf(Scores, currentPlayerScore);

        if (currentIndex < 3)
        {
            var newScore = Scores[currentIndex + 1];
            if (isPlayerA)
            {
                matchInfo.PlayerAScore = newScore;
            }
            else
            {
                matchInfo.PlayerBScore = newScore;
            }

            if(matchInfo.PlayerAScore == "Forty" && matchInfo.PlayerBScore == "Forty")
            {
                matchInfo.Deuce = true;
                matchInfo.Advantage = "";
            }

            await _tennisMatchRepository.AssignScoreAsync(matchInfo);
            return;
        }

        if (currentIndex == 3 && opponentScore != "Forty")
        {
            matchInfo.Winner = request.Player;
            await _tennisMatchRepository.AssignScoreAsync(matchInfo);
            return;
        }


    }
    
    public async Task ResetMatchAsync()
    {
        await _tennisMatchRepository.ResetMatchAsync();
    }
}