using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kairos.API.Context;
using Kairos.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kairos.API.Controllers;

public class EventController : SecurityController
{
    private readonly KairosContext _context;

    public EventController(KairosContext context)
    {
        _context = context;
    }
    
    [HttpGet("/me", Name = "Get the events from a user")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GroupDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    public IActionResult GetEvents()
    {
        
        var userContext = (User) HttpContext.Items["User"];
        if (userContext == null)
        {
            return Unauthorized(new ErrorMessage("Can't create a event", StatusCodes.Status401Unauthorized));
        }
        
        var groupEvent = _context.Groups
            .Include(g => g.Events)
            .Where(g => g.OwnerId == userContext.UserId)
            .Select(g => new GroupDto(g, true, false))
            .ToList();

        if (groupEvent.Count == 0)
        {
            return NotFound(new ErrorMessage("No group found where we can look after your events.", StatusCodes.Status404NotFound));
        }

        return Ok(groupEvent);
    }
    
    /// <summary>
    /// Return the event of an certain group.
    /// </summary>
    /// <param name="
    /// groupId">the id of the group you want to see the event's</param>
    /// <returns></returns>
    [HttpGet("{groupId}", Name = "Get the events of a group")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EventDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    public IActionResult GetEventFromGroupId(string groupId)
    {
        if (string.IsNullOrEmpty(groupId))
        {
            return BadRequest(new ErrorMessage("Group id not specified", StatusCodes.Status400BadRequest));
        }

        int groupIdParsed;
        bool isParsed = int.TryParse(groupId, out groupIdParsed);
        if (!isParsed)
        {
            return BadRequest(new ErrorMessage("Group id is not valid", StatusCodes.Status400BadRequest));
        }

        var events = _context.Events
            .Where(e => e.GroupId == groupIdParsed)
            .Include(e => e.Labels)
            .Select(e => new EventDto(e))
            .ToList();

        if (events.Count == 0)
        {
            return NotFound(new ErrorMessage("No event found for this group", StatusCodes.Status404NotFound));
        }

        return Ok(events);
    }

    [HttpPost("/create", Name = "Create an event")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EventDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> CreateEvent(string groupId, string title, string description, string labels, string sessionDate)
    {
        if (string.IsNullOrEmpty(groupId))
        {
            return BadRequest(new ErrorMessage("Event id not specified", StatusCodes.Status400BadRequest));
        }
        
        var isGroupIdParsed = Int32.TryParse(groupId, out int groupIdParsed);
        if (!isGroupIdParsed)
        {
            return BadRequest(new ErrorMessage("Event id is not valid", StatusCodes.Status400BadRequest));
        }

        var userContext = (User) HttpContext.Items["User"];
        if (userContext == null)
        {
            return Unauthorized(new ErrorMessage("Can't create a event", StatusCodes.Status401Unauthorized));
        }

        if (string.IsNullOrEmpty(title))
        {
            return BadRequest(new ErrorMessage("Title not specified", StatusCodes.Status400BadRequest));
        }

        if (title.Length > 25)
        {
            return BadRequest(new ErrorMessage("Title is too long", StatusCodes.Status400BadRequest));
        }

        if (string.IsNullOrEmpty(description))
        {
            return BadRequest(new ErrorMessage("Description not specified", StatusCodes.Status400BadRequest));
        }

        if (description.Length > 250)
        {
            return BadRequest(new ErrorMessage("Description is too long", StatusCodes.Status400BadRequest));
        }

        if (string.IsNullOrEmpty(labels))
        {
            return BadRequest(new ErrorMessage("Labels not specified", StatusCodes.Status400BadRequest));
        }

        if (string.IsNullOrEmpty(sessionDate))
        {
            return BadRequest(new ErrorMessage("Session date not specified", StatusCodes.Status400BadRequest));
        }

        DateTime.TryParse(sessionDate, out var sessionDateParsed);
        var createEvent = new Event(sessionDateParsed, title, description);

        var labelsSplitted = labels.Split(',');
        if (labelsSplitted.Length > 5)
        {
            return BadRequest(new ErrorMessage("Too many labels", StatusCodes.Status400BadRequest));
        }

        if (labelsSplitted.Length > 0)
        {
            createEvent.Labels = new List<Label>();
            foreach (var label in labelsSplitted)
            {
                var isLabelParsed = Int32.TryParse(label, out var labelId);
                if (!isLabelParsed)
                {
                    return BadRequest(new ErrorMessage("Label id is not valid", StatusCodes.Status400BadRequest));
                }

                var labelDb = await _context.Labels.FirstOrDefaultAsync(l =>
                    label != null && l.LabelId == labelId && l.UserId == userContext.UserId);
                if (labelDb == null)
                {
                    return BadRequest(new ErrorMessage("Label not found or the label owner is not the current user",
                        StatusCodes.Status400BadRequest));
                }

                createEvent.Labels.Add(labelDb);
            }
        }
        
        var groupDb = _context.Groups.FirstOrDefault(g => g.GroupId == groupIdParsed && g.OwnerId == userContext.UserId);
        if (groupDb == null)
        {
            return BadRequest(new ErrorMessage("Group not found or the group owner is not the current user",
                StatusCodes.Status400BadRequest));
        }

        _context.Events.Add(createEvent);
        createEvent.Group = groupDb;
        
        await _context.SaveChangesAsync();

        return Ok(new EventDto(createEvent));
    }
}