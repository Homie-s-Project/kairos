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
    /// Récupère la liste des labels de l'utilisateur
    /// </summary>
    /// <returns>all the labels</returns>
    [HttpGet("me", Name = "Réccupère la liste des labels de l'utilisateur")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LabelDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    public IActionResult GetLabels()
    {
        // On charge l'utilisateur
        var userContext = (User) HttpContext.Items["User"];
        if (userContext == null)
        {
            return Unauthorized(new ErrorMessage("Can't get the labels from this user",
                StatusCodes.Status401Unauthorized));
        }

        // On récupère les labels de l'utilisateur depuis la base de données
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
    /// Récupère les labels d'un événement
    /// </summary>
    /// <param name="eventId">L'id de l'événement</param>
    [HttpGet("event/{eventId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LabelDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    public IActionResult GetEventLabel(string eventId)
    {
        // On vérifie que l'id de l'événement est spécifié
        if (string.IsNullOrEmpty(eventId))
        {
            return BadRequest(new ErrorMessage("Event id not specified", StatusCodes.Status400BadRequest));
        }

        // On charge l'utilisateur
        var userContext = (User) HttpContext.Items["User"];
        if (userContext == null)
        {
            return Unauthorized(new ErrorMessage("Can't get the labels from this event",
                StatusCodes.Status401Unauthorized));
        }

        // On parse l'id de l'événement
        bool isParsed = int.TryParse(eventId, out int eventIdParsed);
        if (!isParsed)
        {
            return BadRequest(new ErrorMessage("Event id is not valid", StatusCodes.Status400BadRequest));
        }

        // On charge l'événement depuis la base de données
        var eventDb = _context.Events
            .Include(e => e.Labels)
            .FirstOrDefault(e => e.EventId == eventIdParsed);

        // S'il n'existe pas, on renvoie une erreur
        if (eventDb == null)
        {
            return NotFound(new ErrorMessage("No event found with this id: " + eventIdParsed,
                StatusCodes.Status404NotFound));
        }

        return Ok(new EventDto(eventDb, true));
    }

    /// <summary>
    /// Suppression d'un label
    /// </summary>
    /// <param name="labelId">l'id du label</param>
    [HttpDelete("delete/{labelId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LabelDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> DeleteLabel(string labelId)
    {
        // On vérifie si le labelId est spécifié
        if (string.IsNullOrEmpty(labelId))
        {
            return BadRequest(new ErrorMessage("Label id not specified", StatusCodes.Status400BadRequest));
        }

        // On charge l'utilisateur
        var userContext = (User) HttpContext.Items["User"];
        if (userContext == null)
        {
            return Unauthorized(new ErrorMessage("Can't delete the label", StatusCodes.Status401Unauthorized));
        }

        // On parse le labelId
        bool isParsed = int.TryParse(labelId, out int labelIdParsed);
        if (!isParsed)
        {
            return BadRequest(new ErrorMessage("Label id is not valid", StatusCodes.Status400BadRequest));
        }

        // On charge le label depuis la base de données
        var labelDb = await _context.Labels
            .Include(l => l.Events)
            .Include(l => l.Groups)
            .Include(l => l.Studies)
            .FirstOrDefaultAsync(l => l.LabelId == labelIdParsed && l.User.UserId == userContext.UserId);

        // Si aucun label n'a été trouvé, on renvoie une erreur
        if (labelDb == null)
        {
            return NotFound(new ErrorMessage("No label found with this id: " + labelIdParsed,
                StatusCodes.Status404NotFound));
        }

        // On supprime le label de la base de données
        _context.Labels.Remove(labelDb);
        await _context.SaveChangesAsync();

        // On le renvoie si jamais le front en a besoin.
        return Ok(new LabelDto(labelDb, true, true));
    }

    /// <summary>
    /// Modification d'un label
    /// </summary>
    /// <param name="labelId">L'id du label</param>
    /// <param name="labelName">Le nom du label</param>
    [HttpPut("update/{labelId}", Name = "Modification d'un label")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LabelDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    public async Task<ActionResult> UpdateLabel(string labelId, [FromForm] string labelName)
    {
        // On vérifie si le labelId est spécifié
        if (string.IsNullOrEmpty(labelId))
        {
            return BadRequest(new ErrorMessage("Label id not specified", StatusCodes.Status400BadRequest));
        }

        // On charge l'utilisateur
        var userContext = (User) HttpContext.Items["User"];
        if (userContext == null)
        {
            return Unauthorized(new ErrorMessage("Can't update the label", StatusCodes.Status401Unauthorized));
        }

        // On parse le labelId
        bool isParsed = int.TryParse(labelId, out int labelIdParsed);
        if (!isParsed)
        {
            return BadRequest(new ErrorMessage("Label id is not valid", StatusCodes.Status400BadRequest));
        }

        // On charge le label depuis la base de données
        var labelDb = await _context.Labels
            .Include(l => l.Events)
            .Include(l => l.Groups)
            .Include(l => l.Studies)
            .FirstOrDefaultAsync(l => l.LabelId == labelIdParsed && l.User.UserId == userContext.UserId);

        // Si aucun label n'a été trouvé, on renvoie une erreur
        if (labelDb == null)
        {
            return NotFound(new ErrorMessage("No label found with this id: " + labelIdParsed,
                StatusCodes.Status404NotFound));
        }

        // On met à jour le label
        labelDb.LabelTitle = labelName;
        await _context.SaveChangesAsync();

        return Ok(new LabelDto(labelDb));
    }

    /// <summary>
    /// Récupération des labels d'un groupe
    /// </summary>
    /// <param name="groupId">L'id du groupe</param>
    [HttpGet("group/{groupId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GroupDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    public IActionResult GetGroupLabel(string groupId)
    {
        // On vérifie si l'id du groupe est spécifié
        if (string.IsNullOrEmpty(groupId))
        {
            return BadRequest(new ErrorMessage("Group id not specified", StatusCodes.Status400BadRequest));
        }

        // On charge l'utilisateur
        var userContext = (User) HttpContext.Items["User"];
        if (userContext == null)
        {
            return Unauthorized(
                new ErrorMessage("Can't get the label of this group", StatusCodes.Status401Unauthorized));
        }

        // On parse l'id du groupe
        bool isParsed = int.TryParse(groupId, out int groupIdParsed);
        if (!isParsed)
        {
            return BadRequest(new ErrorMessage("Group id is not valid", StatusCodes.Status400BadRequest));
        }

        // On charge le groupe depuis la base de données
        var group = _context.Groups
            .Include(g => g.Labels)
            .FirstOrDefault(g => g.OwnerId == userContext.UserId && g.GroupId == groupIdParsed);

        // Si aucun groupe n'a été trouvé, on renvoie une erreur
        if (group == null)
        {
            return NotFound(new ErrorMessage("No group found with this id: " + groupIdParsed,
                StatusCodes.Status404NotFound));
        }

        return Ok(new GroupDto(group, true));
    }

    /// <summary>
    /// Création d'un label pour l'utilisateur
    /// </summary>
    /// <param name="labelName">Le nom du label/param>
    [HttpPost("create", Name = "Création d'un label")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LabelDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> CreateLabel([FromForm] string labelName)
    {
        // On vérifie si le labelName est spécifié
        if (string.IsNullOrEmpty(labelName))
        {
            return BadRequest(new ErrorMessage("No name specified", StatusCodes.Status400BadRequest));
        }

        // On vérifie que la longueur du labelName n'est pas trop longue
        if (labelName.Length > 50)
        {
            return BadRequest(new ErrorMessage("Label name is too long", StatusCodes.Status400BadRequest));
        }

        // On charge l'utilisateur
        var userContext = (User) HttpContext.Items["User"];
        if (userContext == null)
        {
            return Unauthorized(new ErrorMessage("Can't create a label", StatusCodes.Status401Unauthorized));
        }

        // On créé le label
        var label = new Label(labelName, userContext.UserId);

        // On ajoute le label crée à la base de données
        _context.Labels.Add(label);
        await _context.SaveChangesAsync();

        return Ok(new LabelDto(label, false));
    }
}