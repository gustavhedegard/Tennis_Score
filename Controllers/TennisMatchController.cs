using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class TennisMatchController : ControllerBase
{

    [HttpGet("get-score")]
    public async Task<IActionResult> GetScore()
    {
         return await tennisMatchService.GetScore(response);
    }

    [HttpPost("assign-point")]
    public async Task<IActionResult> AssignPoint([FromBody] AssignPointDto request)
    {
        await tennisMatchService.AssignPoint(request);
    }

}