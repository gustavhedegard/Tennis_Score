using Xunit;
using Moq;
using System.Threading.Tasks;

public class TennisMatchServiceTests
{
    private readonly Mock<ITennisMatchRepository> _repoMock;
    private readonly TennisMatchService _service;

    public TennisMatchServiceTests()
    {
        _repoMock = new Mock<ITennisMatchRepository>();
        _service = new TennisMatchService(_repoMock.Object);
    }

    [Fact]
    public async Task AssignPoint_PlayerAWins_WhenOpponentBelowForty()
    {
        var matchInfo = new MatchInfoDto
        {
            PlayerAScore = Score.Forty,
            PlayerBScore = Score.Thirty,
            Advantage = null,
            Deuce = false,
            Winner = null
        };

        _repoMock.Setup(r => r.GetMatchInfoAsync()).ReturnsAsync(matchInfo);
        _repoMock.Setup(r => r.AssignScoreAsync(It.IsAny<MatchInfoDto>())).Returns(Task.CompletedTask);

        var request = new AssignPointDto { Player = Player.A };
        await _service.AssignPointAsync(request);

        Assert.Equal(Player.A, matchInfo.Winner);
    }

    [Fact]
    public async Task AssignPoint_ToDeuce_WhenBothReachForty()
    {
        var matchInfo = new MatchInfoDto
        {
            PlayerAScore = Score.Thirty,
            PlayerBScore = Score.Forty,
            Advantage = null,
            Deuce = false,
            Winner = null
        };

        _repoMock.Setup(r => r.GetMatchInfoAsync()).ReturnsAsync(matchInfo);
        _repoMock.Setup(r => r.AssignScoreAsync(It.IsAny<MatchInfoDto>())).Returns(Task.CompletedTask);

        var request = new AssignPointDto { Player = Player.A };
        await _service.AssignPointAsync(request);

        Assert.True(matchInfo.Deuce);
        Assert.Null(matchInfo.Winner);
    }

    [Fact]
    public async Task AssignPoint_PlayerWins_WhenHasAdvantage()
    {
        var matchInfo = new MatchInfoDto
        {
            PlayerAScore = Score.Forty,
            PlayerBScore = Score.Forty,
            Advantage = Player.A,
            Deuce = false,
            Winner = null
        };

        _repoMock.Setup(r => r.GetMatchInfoAsync()).ReturnsAsync(matchInfo);
        _repoMock.Setup(r => r.AssignScoreAsync(It.IsAny<MatchInfoDto>())).Returns(Task.CompletedTask);

        var request = new AssignPointDto { Player = Player.A };
        await _service.AssignPointAsync(request);

        Assert.Equal(Player.A, matchInfo.Winner);
        Assert.Null(matchInfo.Advantage);
    }

    [Fact]
    public async Task AssignPoint_PlayerGainsAdvantage_FromDeuce()
    {
        var matchInfo = new MatchInfoDto
        {
            PlayerAScore = Score.Forty,
            PlayerBScore = Score.Forty,
            Advantage = null,
            Deuce = true,
            Winner = null
        };

        _repoMock.Setup(r => r.GetMatchInfoAsync()).ReturnsAsync(matchInfo);
        _repoMock.Setup(r => r.AssignScoreAsync(It.IsAny<MatchInfoDto>())).Returns(Task.CompletedTask);

        var request = new AssignPointDto { Player = Player.A };
        await _service.AssignPointAsync(request);

        Assert.Equal(Player.A, matchInfo.Advantage);
        Assert.False(matchInfo.Deuce);
        Assert.Null(matchInfo.Winner);
    }

    [Fact]
    public async Task AssignPoint_PlayerLosesAdvantage_BackToDeuce()
    {
        var matchInfo = new MatchInfoDto
        {
            PlayerAScore = Score.Forty,
            PlayerBScore = Score.Forty,
            Advantage = Player.A,
            Deuce = false,
            Winner = null
        };

        _repoMock.Setup(r => r.GetMatchInfoAsync()).ReturnsAsync(matchInfo);
        _repoMock.Setup(r => r.AssignScoreAsync(It.IsAny<MatchInfoDto>())).Returns(Task.CompletedTask);

        var request = new AssignPointDto { Player = Player.B };
        await _service.AssignPointAsync(request);

        Assert.Null(matchInfo.Advantage);
        Assert.True(matchInfo.Deuce);
        Assert.Null(matchInfo.Winner);
    }

    [Fact]
    public async Task ResetMatch_CallsRepository()
    {
        _repoMock.Setup(r => r.ResetMatchAsync()).Returns(Task.CompletedTask).Verifiable();

        await _service.ResetMatchAsync();

        _repoMock.Verify(r => r.ResetMatchAsync(), Times.Once);
    }
}
