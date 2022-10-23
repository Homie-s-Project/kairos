using Microsoft.AspNetCore.Mvc;

namespace Kairos.API.Controllers;

[Produces("application/json")]
[ApiController]
[Route("api/[controller]")]
public class BaseController : ControllerBase
{
}