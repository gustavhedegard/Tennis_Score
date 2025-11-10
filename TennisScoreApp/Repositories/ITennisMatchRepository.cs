public interface ITennisMatchRepository
{
    public Task<MatchInfoDto> GetMatchInfoAsync();
    public Task AssignScoreAsync(MatchInfoDto matchInfo);
    public Task ResetMatchAsync();
}