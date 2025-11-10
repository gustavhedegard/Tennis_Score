public class MatchInfoDto
{
    public Score PlayerAScore { get; set; }
    public Score PlayerBScore { get; set; }
    public Player? Advantage { get; set; }
    public Player? Winner { get; set; }
    public bool Deuce { get; set; } = false;
}