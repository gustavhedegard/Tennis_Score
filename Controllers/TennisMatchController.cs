using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class TennisMatchController : ControllerBase
{
    private readonly ITennisMatchService _tennisMatchService;

    public TennisMatchController(ITennisMatchService tennisMatchService)
    {
        _tennisMatchService = tennisMatchService;
    }

    [HttpGet("get-score")]
    public async Task<ActionResult<MatchInfoDto>> GetScore()
    {
        var response = await _tennisMatchService.GetScoreAsync();
        return Ok(response);
    }

    [HttpPost("assign-point")]
    public async Task<IActionResult> AssignPoint([FromBody] AssignPointDto request)
    {
        await _tennisMatchService.AssignPointAsync(request);

        return Ok();
    }

    [HttpDelete("new-game")]
    public async Task<IActionResult> NewGame()
    {
        await _tennisMatchService.NewGameAsync();
        return Ok();
    }

}