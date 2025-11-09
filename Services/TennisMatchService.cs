
public class TennisMatchService : ITennisMatchService
{
    private readonly ITennisMatchRepository _tennisMatchRepository;

    public TennisMatchService(ITennisMatchRepository tennisMatchRepository)
    {
        _tennisMatchRepository = tennisMatchRepository;
    }
    public Task AssignPointAsync(AssignPointDto request)
    {
        throw new NotImplementedException();
    }

    public async Task<ScoreDto> GetScoreAsync()
    {
        var (player1, player2) = await _tennisMatchRepository.GetScoreAsync();

        var score = new ScoreDto()
        {
            Player1 = player1,
            Player2 = player2
        };

        return score;
    }
}