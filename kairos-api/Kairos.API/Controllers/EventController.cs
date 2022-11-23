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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Event))]
    public IActionResult GetEventFromGroupId(string groupId)
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

        // TODO: Check if the user is part of the group
        var events = _context.Groups
            .Where(g => g.GroupId == groupIdParsed)
            .Include(g => g.Event.Labels)
            .Select(g => g.Event)
            .ToList();

        if (events.Count == 0)
        {
            return BadRequest("No event found for this group");
        }

        return Ok(events);
    }
}