using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QuorumMind.Infrastructure.RemoteConfiguration.ExampleApp.App.Models;

namespace QuorumMind.Infrastructure.RemoteConfiguration.ExampleApp.Controllers;

[ApiController]
[Route("api/example")]
public class ExampleController : Controller
{
    private readonly IOptionsMonitor<EmailConfig> _config;

    public ExampleController(IOptionsMonitor<EmailConfig> config)
    {
        _config = config;
    }
    
    [HttpGet]
    [Route("email-settings")]
    public IActionResult Index()
    {
        return Ok(_config.CurrentValue);
    }
}