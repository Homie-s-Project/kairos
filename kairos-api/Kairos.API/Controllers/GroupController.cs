using System.Linq;
using System.Threading.Tasks;
using Kairos.API.Context;
using Kairos.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kairos.API.Controllers;

[Microsoft.AspNetCore.Components.Route("group")]
public class GroupController : SecurityController
{
    private readonly KairosContext _context;

    public GroupController(KairosContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get all the group where the user is in
    /// </summary>
    /// <returns>the list of the groups</returns>
    [HttpGet("me", Name = "Get all group where connected user is in")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Group))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Group))]
    public IActionResult GetGroups()
    {
        var userContext = (User) HttpContext.Items["User"];

        var groups = _context.Groups.Where(g => g.UserId == userContext.UserId).ToList();
        if (groups.Count > 0)
        {
            return Ok(groups);
        }

        return NotFound("No group found");
    }

    /// <summary>
    /// Get all the personal group where the user is the owner and the group is private
    /// </summary>
    /// <returns>the personal groups</returns>
    [HttpGet("personal", Name = "Get all the personal groups")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Group))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Group))]
    public IActionResult GetPrivateGroup()
    {
        var userConterxt = (User) HttpContext.Items["User"];

        var groups = _context.Groups.Where(g => g.GroupsIsPrivate && g.UserId == userConterxt.UserId)
            .ToList();
        if (groups.Count > 0)
        {
            return Ok(groups);
        }

        return NotFound("No group found");
    }

    [HttpPost("create", Name = "Create a group")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Group))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Group))]
    public async Task<IActionResult> CreateGroup(string groupName)
    {
        var userConterxt = (User) HttpContext.Items["User"];

        if (userConterxt == null)
        {
            return Unauthorized("Can't create a group");
        }

        if (string.IsNullOrEmpty(groupName))
        {
            return BadRequest("No name specified");
        }

        var group = new Group(groupName, userConterxt.UserId);
        _context.Groups.Add(group);
        await _context.SaveChangesAsync();

        return Ok(group);
    }
}