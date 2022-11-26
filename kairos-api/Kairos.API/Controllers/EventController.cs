using System.Linq;
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

        var events = _context.Groups
            .Where(g => g.GroupId == groupIdParsed)
            .Include(g => g.Event.Labels)
            .Select(g => new EventDto(g.Event))
            .ToList();

        if (events.Count == 0)
        {
            return NotFound(new ErrorMessage("No event found for this group", StatusCodes.Status404NotFound));
        }

        return Ok(events);
    }
}