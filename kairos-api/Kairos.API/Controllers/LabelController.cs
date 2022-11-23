using System.Linq;
using System.Threading.Tasks;
using Kairos.API.Context;
using Kairos.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kairos.API.Controllers;

public class LabelController : SecurityController
{
    private readonly KairosContext _context;

    public LabelController(KairosContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get all the created label from the user connected
    /// </summary>
    /// <returns>all the labels</returns>
    [HttpGet("me", Name = "Get all your created label")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Label))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Label))]
    public IActionResult GetLabels()
    {
        var userContext = (User) HttpContext.Items["User"];

        var labels = _context.Labels.Where(l => l.UserId == userContext.UserId)
            .AsSplitQuery()
            .Include(l => l.Groups)
            .Include(l => l.Events)
            .Include(l => l.Studies)
            .Select(l => new LabelDto(l))
            .ToList();

        if (labels.Count > 0)
        {
            return Ok(labels);
        }

        return NotFound("no labels created by you found");
    }

    /// <summary>
    /// Get the labels from a event
    /// </summary>
    /// <param name="eventId">the id of the event</param>
    /// <returns>the labels</returns>
    [HttpGet("/label/event/{eventId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Label))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Label))]
    public IActionResult GetEventLabel(string eventId)
    {
        if (string.IsNullOrEmpty(eventId))
        {
            return BadRequest("Event id not specified");
        }

        int eventIdParsed;
        bool isParsed = int.TryParse(eventId, out eventIdParsed);
        if (!isParsed)
        {
            return BadRequest("Event id is not valid");
        }

        // TODO: Vérifier que l'utilisateur aille accès à cette event.
        var events = _context.Events.Where(e => e.EventId == eventIdParsed)
            .Include(e => e.Labels)
            .ToList();

        if (events.Count == 0)
        {
            return NotFound("No event found with this id: " + eventIdParsed);
        }

        return Ok(events);
    }

    /// <summary>
    /// Get the labels from a grroup
    /// </summary>
    /// <param name="groupId">the id of the group</param>
    /// <returns>the labels</returns>
    [HttpGet("/label/group/{groupId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Label))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Label))]
    public IActionResult GetGroupLabel(string groupId)
    {
        if (string.IsNullOrEmpty(groupId))
        {
            return BadRequest("Group id not specified");
        }

        int groupIdParsed;
        bool isParsed = int.TryParse(groupId, out groupIdParsed);
        if (!isParsed)
        {
            return BadRequest("Group id is not valid");
        }

        // TODO: Vérifier que l'utilisateur aille accès à cette event.
        var groups = _context.Groups.Where(g => g.GroupId == groupIdParsed)
            .Include(g => g.Labels)
            .Select(g => new
            {
                g.GroupId,
                g.GroupName,
                ownderId = g.UserId,
            })
            .ToList();

        if (groups.Count == 0)
        {
            return NotFound("No group found with this id: " + groupIdParsed);
        }

        return Ok(groups);
    }

    /// <summary>
    /// Create a label
    /// </summary>
    /// <param name="labelName">the name of the label</param>
    /// <returns>the created label</returns>
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