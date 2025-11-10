public interface ITennisMatchService
{
    public Task<MatchInfoDto> GetScoreAsync();
    public Task AssignPointAsync(AssignPointDto request);
}