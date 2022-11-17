using System.Linq;
using System.Threading.Tasks;
using Kairos.API.Context;
using Kairos.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kairos.API.Controllers;

public class LabelController : SecurityController
{
    private readonly KairosContext _context;

    public LabelController(KairosContext context)
    {
        _context = context;
    }

    [HttpGet("me", Name = "Get all your created label")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Label))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Label))]
    public IActionResult GetLabels()
    {
        var userContext = (User) HttpContext.Items["User"];

        var labels = _context.Labels.Where(l => l.UserId == userContext.UserId).ToList();
        if (labels.Count > 0)
        {
            return Ok(labels);
        }

        return NotFound("no labels created by you found");
    }

    [HttpPost("create", Name = "Create a label")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Label))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Label))]
    public async Task<IActionResult> CreateLabel(string labelName)
    {
        var userConterxt = (User) HttpContext.Items["User"];

        if (userConterxt == null)
        {
            return Unauthorized("Can't create a group");
        }
        
        if (string.IsNullOrEmpty(labelName))
        {
            return BadRequest("No name specified");
        }

        var label = new Label(labelName, userConterxt.UserId);
        _context.Labels.Add(label);
        await _context.SaveChangesAsync();

        return Ok(label);
    }
}