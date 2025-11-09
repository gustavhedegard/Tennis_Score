public interface ITennisMatchRepository
{
    public Task<(string player1, string player2)> GetScoreAsync();
}