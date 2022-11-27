using Kairos.API.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace Kairos.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class SecurityController : BaseController
{
}