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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LabelDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    public IActionResult GetLabels()
    {
        var userContext = (User) HttpContext.Items["User"];

        var labels = _context.Labels.Where(l => l.UserId == userContext.UserId)
            .AsSplitQuery()
            .Include(l => l.Groups)
            .Include(l => l.Events)
            .Include(l => l.Studies)
            .Select(l => new LabelDto(l, true, false))
            .ToList();

        if (labels.Count > 0)
        {
            return Ok(labels);
        }

        return NotFound(new ErrorMessage("No labels created by you found", StatusCodes.Status404NotFound));
    }

    /// <summary>
    /// Get the labels from a event
    /// </summary>
    /// <param name="eventId">the id of the event</param>
    /// <returns>the labels</returns>
    [HttpGet("/label/event/{eventId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LabelDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    public IActionResult GetEventLabel(string eventId)
    {
        if (string.IsNullOrEmpty(eventId))
        {
            return BadRequest(new ErrorMessage("Event id not specified", StatusCodes.Status400BadRequest));
        }
        
        var userConterxt = (User) HttpContext.Items["User"];
        if (userConterxt == null)
        {
            return Unauthorized(new ErrorMessage("Can't get the labels from this event", StatusCodes.Status401Unauthorized));
        }

        int eventIdParsed;
        bool isParsed = int.TryParse(eventId, out eventIdParsed);
        if (!isParsed)
        {
            return BadRequest(new ErrorMessage("Event id is not valid", StatusCodes.Status400BadRequest));
        }

        var eventDb = _context.Events
            .Include(e => e.Labels)
            .FirstOrDefault(e => e.EventId == eventIdParsed);

        if (eventDb == null)
        {
            return NotFound(new ErrorMessage("No event found with this id: " + eventIdParsed, StatusCodes.Status404NotFound));
        }

        return Ok(new EventDto(eventDb, true));
    }
    
    [HttpDelete("delete/{labelId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LabelDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> DeleteLabel(string labelId)
    {
        if (string.IsNullOrEmpty(labelId))
        {
            return BadRequest(new ErrorMessage("Label id not specified", StatusCodes.Status400BadRequest));
        }
        
        var userConterxt = (User) HttpContext.Items["User"];
        if (userConterxt == null)
        {
            return Unauthorized(new ErrorMessage("Can't delete the label", StatusCodes.Status401Unauthorized));
        }

        bool isParsed = int.TryParse(labelId, out int labelIdParsed);
        if (!isParsed)
        {
            return BadRequest(new ErrorMessage("Label id is not valid", StatusCodes.Status400BadRequest));
        }

        var labelDb = await _context.Labels
            .Include(l => l.Events)
            .Include(l => l.Groups)
            .Include(l => l.Studies)
            .FirstOrDefaultAsync(l => l.LabelId == labelIdParsed && l.User.UserId == userConterxt.UserId);

        if (labelDb == null)
        {
            return NotFound(new ErrorMessage("No label found with this id: " + labelIdParsed, StatusCodes.Status404NotFound));
        }

        _context.Labels.Remove(labelDb);
        await _context.SaveChangesAsync();

        return Ok(new LabelDto(labelDb, true, true));
    }

    [HttpPut("update/{labelId}", Name = "Update a label")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LabelDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    public async Task<ActionResult> UpdateLabel(string labelId, string labelName)
    {
        if (string.IsNullOrEmpty(labelId))
        {
            return BadRequest(new ErrorMessage("Label id not specified", StatusCodes.Status400BadRequest));
        }
        
        var userContext = (User) HttpContext.Items["User"];
        if (userContext == null)
        {
            return Unauthorized(new ErrorMessage("Can't update the label", StatusCodes.Status401Unauthorized));
        }

        bool isParsed = int.TryParse(labelId, out int labelIdParsed);
        if (!isParsed)
        {
            return BadRequest(new ErrorMessage("Label id is not valid", StatusCodes.Status400BadRequest));
        }

        var labelDb = await _context.Labels
            .Include(l => l.Events)
            .Include(l => l.Groups)
            .Include(l => l.Studies)
            .FirstOrDefaultAsync(l => l.LabelId == labelIdParsed && l.User.UserId == userContext.UserId);

        if (labelDb == null)
        {
            return NotFound(new ErrorMessage("No label found with this id: " + labelIdParsed, StatusCodes.Status404NotFound));
        }

        labelDb.LabelTitle = labelName;
        await _context.SaveChangesAsync();

        return Ok(new LabelDto(labelDb));
    }

    /// <summary>
    /// Get the labels from a grroup
    /// </summary>
    /// <param name="groupId">the id of the group</param>
    /// <returns>the labels</returns>
    [HttpGet("/label/group/{groupId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GroupDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    public IActionResult GetGroupLabel(string groupId)
    {
        if (string.IsNullOrEmpty(groupId))
        {
            return BadRequest(new ErrorMessage("Group id not specified", StatusCodes.Status400BadRequest));
        }
        
        var userConterxt = (User) HttpContext.Items["User"];
        if (userConterxt == null)
        {
            return Unauthorized(new ErrorMessage("Can't get the label of this group", StatusCodes.Status401Unauthorized));
        }

        int groupIdParsed;
        bool isParsed = int.TryParse(groupId, out groupIdParsed);
        if (!isParsed)
        {
            return BadRequest(new ErrorMessage("Group id is not valid", StatusCodes.Status400BadRequest));
        }

        var group = _context.Groups
            .Include(g => g.Labels)
            .FirstOrDefault(g => g.OwnerId == userConterxt.UserId && g.GroupId == groupIdParsed);

        if (group == null)
        {
            return NotFound(new ErrorMessage("No group found with this id: " + groupIdParsed, StatusCodes.Status404NotFound));
        }

        var groupDto = new GroupDto(group, true);
        return Ok(groupDto);
    }

    /// <summary>
    /// Create a label
    /// </summary>
    /// <param name="labelName">the name of the label</param>
    /// <returns>the created label</returns>
    [HttpPost("create", Name = "Create a label")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LabelDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> CreateLabel(string labelName)
    {
        var userConterxt = (User) HttpContext.Items["User"];

        if (userConterxt == null)
        {
            return Unauthorized(new ErrorMessage("Can't create a label", StatusCodes.Status401Unauthorized));
        }

        if (string.IsNullOrEmpty(labelName))
        {
            return BadRequest(new ErrorMessage("No name specified", StatusCodes.Status400BadRequest));
        }

        if (labelName.Length > 50)
        {
            return BadRequest(new ErrorMessage("Label name is too long", StatusCodes.Status400BadRequest));
        }

        var label = new Label(labelName, userConterxt.UserId);
        _context.Labels.Add(label);
        await _context.SaveChangesAsync();

        return Ok(new LabelDto(label, false));
    }
}