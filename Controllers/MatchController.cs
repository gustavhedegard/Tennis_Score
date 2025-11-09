using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class MatchController : ControllerBase
{

    [HttpGet("get-score")]
    public IActionResult GetScore()
    {
        return Ok("Love");
    }



}