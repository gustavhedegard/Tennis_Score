public interface ITennisMatchService
{
    public Task<ScoreDto> GetScoreAsync();
    public Task AssignPointAsync(AssignPointDto request);
}