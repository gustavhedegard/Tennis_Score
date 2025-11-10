public interface ITennisMatchRepository
{
    public Task<MatchInfoDto> GetScoreAsync();
    public Task AssignScoreAsync(MatchInfoDto matchInfo);
}