using System;
using System.Linq;
using System.Threading.Tasks;
using Kairos.API.Context;
using Kairos.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    /// Récupère la liste des groupes de l'utilisateur
    /// </summary>
    [HttpGet("me", Name = "Récupère la liste des groupes de l'utilisateur")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GroupDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    public IActionResult GetGroups()
    {
        // On charge l'utilisateur
        var userContext = (User) HttpContext.Items["User"];
        if (userContext == null)
        {
            return Unauthorized(new ErrorMessage("L'utilisateur ne peut pas récupérer les groupes",
                StatusCodes.Status401Unauthorized));
        }

        // On charge l'utilisateur
        var user = _context.Users.FirstOrDefault(u => u.UserId == userContext.UserId);

        // On charge les groupes où l'utilisateur est membre
        var groups = _context.Groups.Where(g => g.Users.Contains(user) || g.OwnerId == userContext.UserId)
            .AsSplitQuery()
            .Include(g => g.Events)
            .Include(g => g.Labels)
            .Include(g => g.Users)
            .Select(g => new GroupDto(g))
            .ToList();

        // Si aucun groupe n'a été trouvé
        if (groups.Count == 0)
        {
            return NotFound(new ErrorMessage("No group found", StatusCodes.Status404NotFound));
        }

        return Ok(groups);
    }

    /// <summary>
    /// Récupère les groupes dont l'utilisateur est propriétaire
    /// </summary>
    /// <returns>the personal groups</returns>
    [HttpGet("personal", Name = "Récupère les groupes dont l'utilisateur est propriétaire")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GroupDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    public IActionResult GetPrivateGroup()
    {
        // On charge l'utilisateur
        var userContext = (User) HttpContext.Items["User"];
        if (userContext == null)
        {
            return Unauthorized(new ErrorMessage("L'utilisateur ne peut pas récupérer les groupes",
                StatusCodes.Status401Unauthorized));
        }

        // On charge les groupes où l'utilisateur est propriétaire
        var groups = _context.Groups.Where(g => g.GroupsIsPrivate && g.OwnerId == userContext.UserId)
            .Select(g => new GroupDto(g))
            .ToList();

        // Si aucun groupe n'a été trouvé
        if (groups.Count == 0)
        {
            return NotFound(new ErrorMessage("No group found", StatusCodes.Status404NotFound));
        }

        return Ok(groups);
    }

    /// <summary>
    /// Création d'un groupe
    /// </summary>
    /// <param name="groupName">le nom du groupe</param>
    [HttpPost("create", Name = "Create a group")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GroupDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> CreateGroup(string groupName)
    {
        // On charge l'utilisateur
        var userContext = (User) HttpContext.Items["User"];
        if (userContext == null)
        {
            return Unauthorized(new ErrorMessage("Can't create a group", StatusCodes.Status401Unauthorized));
        }

        // On vérifie que le nom du groupe n'est pas null ou vide
        if (string.IsNullOrEmpty(groupName))
        {
            return BadRequest(new ErrorMessage("Can't create a group", StatusCodes.Status400BadRequest));
        }

        // On vérifie que le nom du groupe n'est pas plus long que 50 caractères
        if (groupName.Length > 50)
        {
            return BadRequest(new ErrorMessage("The name of the group is too long", StatusCodes.Status400BadRequest));
        }

        // On crée le groupe
        var group = new Group(groupName, userContext.UserId);

        // On ajoute le groupe à la base de données
        _context.Groups.Add(group);
        await _context.SaveChangesAsync();

        return Ok(new GroupDto(group, false));
    }

    /// <summary>
    /// Suppression d'un groupe
    /// </summary>
    /// <param name="groupId">L'id du groupe</param>
    [HttpDelete("delete/{groupId}", Name = "Suppression d'un groupe")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GroupDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> DeleteGroup(string groupId)
    {
        // On vérifie que l'id du groupe n'est pas null ou vide
        if (string.IsNullOrEmpty(groupId))
        {
            return BadRequest(new ErrorMessage("No group id found", StatusCodes.Status400BadRequest));
        }

        // On charge l'utilisateur
        var userContext = (User) HttpContext.Items["User"];
        if (userContext == null)
        {
            return Unauthorized(new ErrorMessage("Can't delete a group", StatusCodes.Status401Unauthorized));
        }

        // On parse l'id du groupe de string à int
        bool isParsed = Int32.TryParse(groupId, out var groupIdParsed);
        if (!isParsed)
        {
            return BadRequest(new ErrorMessage("Can't parse the group id", StatusCodes.Status400BadRequest));
        }

        // On charge le groupe
        var group = await _context.Groups
            .Include(g => g.Events)
            .FirstOrDefaultAsync(g => g.GroupId == groupIdParsed && g.OwnerId == userContext.UserId);

        // S'il est null, alors on retourne une erreur pour indiquer que le groupe n'a pas été trouvé.
        if (group == null)
        {
            return NotFound(new ErrorMessage("Can't delete a group", StatusCodes.Status404NotFound));
        }

        // On supprime le groupe de la base de données.
        _context.Groups.Remove(group);
        await _context.SaveChangesAsync();

        return Ok(new GroupDto(group, false));
    }
}