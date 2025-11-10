using System.Text.RegularExpressions;

public class TennisMatchService : ITennisMatchService
{
    private readonly ITennisMatchRepository _tennisMatchRepository;
    private readonly string[] Scores = { "Love", "Fifteen", "Thirty", "Forty" };

    public TennisMatchService(ITennisMatchRepository tennisMatchRepository)
    {
        _tennisMatchRepository = tennisMatchRepository;
    }

    public async Task<MatchInfoDto> GetScoreAsync()
    {
        var matchInfo = await _tennisMatchRepository.GetScoreAsync();
        return matchInfo;
    }
    public async Task AssignPointAsync(AssignPointDto request)
    {
        var matchInfo = await _tennisMatchRepository.GetScoreAsync();

        if (matchInfo.Winner != null)
            return;

        bool isPlayerA = request.Player == "A";
        string currentPlayerScore = isPlayerA ? matchInfo.PlayerAScore : matchInfo.PlayerBScore;
        string opponentScore = isPlayerA ? matchInfo.PlayerBScore : matchInfo.PlayerAScore;

        if (matchInfo.PlayerAScore == "Forty" && matchInfo.PlayerBScore == "Forty")
        {
            if (matchInfo.Advantage == request.Player)
            {
                matchInfo.Winner = request.Player;
            }
            else if (string.IsNullOrEmpty(matchInfo.Advantage))
            {
                matchInfo.Advantage = request.Player;
            }
            else
            {
                matchInfo.Advantage = "";
            }

            await _tennisMatchRepository.AssignScoreAsync(matchInfo);
            return;
        }

        int currentIndex = Array.IndexOf(Scores, currentPlayerScore);

        // om index är under 3 lägg på 1 för ny poäng, skriv ner ny poäng, hoppa ur metoden (return)
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
    
    public async Task NewGameAsync()
    {
        await _tennisMatchRepository.NewGameAsync();
    }


}