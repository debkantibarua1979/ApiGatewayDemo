using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController: ControllerBase
{
    private static readonly string[] Users = new[]
    {
        "Alice", "Bob", "Charlie", "Diana", "Eve"
    };

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(Users);
    }
    
}