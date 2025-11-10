public interface ITennisMatchService
{
    public Task<MatchInfoDto> GetMatchInfoAsync();
    public Task AssignPointAsync(AssignPointDto request);
    public Task ResetMatchAsync();
}