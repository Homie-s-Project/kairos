using Kairos.API.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace midas_api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class SecurityController : BaseController
{
}