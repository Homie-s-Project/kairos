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

    /// <summary>
    /// Permet de récupérer la liste des événements
    /// </summary>
    [HttpGet("me", Name = "Permet de récupérer la liste des événements")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GroupDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    public IActionResult GetEvents()
    {
        // On charge l'utilisateur
        var userContext = (User) HttpContext.Items["User"];
        if (userContext == null)
        {
            return Unauthorized(new ErrorMessage("Can't create a event", StatusCodes.Status401Unauthorized));
        }

        // On charge les événements depuis la base de données
        var groupEvent = _context.Groups
            .Include(g => g.Events)
            .Include("Events.Labels")
            .Where(g => g.OwnerId == userContext.UserId)
            .Select(g => new GroupDto(g, true, false))
            .ToList();

        // Si aucun événement n'est trouvé, on retourne une erreur 404
        if (groupEvent.Count == 0)
        {
            return NotFound(new ErrorMessage("No group found where we can look after your events.",
                StatusCodes.Status404NotFound));
        }

        return Ok(groupEvent);
    }

    /// <summary>
    /// On retourne les événements d'un groupe en particulier
    /// </summary>
    /// <param name="groupId">L'id d'un groupe</param>
    [HttpGet("{groupId}", Name = "Get the events of a group")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EventDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    public IActionResult GetEventFromGroupId(string groupId)
    {
        // Si l'id du groupe est null, on retourne une erreur 400
        if (string.IsNullOrEmpty(groupId))
        {
            return BadRequest(new ErrorMessage("Group id not specified", StatusCodes.Status400BadRequest));
        }

        // On parse l'id du groupe de string à int
        bool isParsed = int.TryParse(groupId, out int groupIdParsed);
        if (!isParsed)
        {
            // Si l'id du groupe n'est pas un nombre, on retourne une erreur 400
            return BadRequest(new ErrorMessage("Group id is not valid", StatusCodes.Status400BadRequest));
        }

        // On récupère les événements du groupe
        var events = _context.Events
            .Where(e => e.GroupId == groupIdParsed)
            .Include(e => e.Labels)
            .Select(e => new EventDto(e))
            .ToList();

        // Si aucun événement n'est trouvé, on retourne une erreur 404
        if (events.Count == 0)
        {
            return NotFound(new ErrorMessage("No event found for this group", StatusCodes.Status404NotFound));
        }

        return Ok(events);
    }

    /// <summary>
    /// Mettre à jour un événement
    /// </summary>
    /// <param name="eventId">L'id de l'événement</param>
    [HttpDelete("delete/{eventId}", Name = "Supprimer un événement")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EventDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> DeleteEvent(string eventId)
    {
        // Si l'id de l'événement est null, on retourne une erreur 400
        if (string.IsNullOrEmpty(eventId))
        {
            return BadRequest(new ErrorMessage("Event id not specified", StatusCodes.Status400BadRequest));
        }

        // On charge l'utilisateur
        var userContext = (User) HttpContext.Items["User"];
        if (userContext == null)
        {
            return Unauthorized(new ErrorMessage("Can't create a event", StatusCodes.Status401Unauthorized));
        }

        // On parse l'id de l'événement de string à int
        bool isParsed = int.TryParse(eventId, out int eventIdParsed);
        if (!isParsed)
        {
            return BadRequest(new ErrorMessage("Event id is not valid", StatusCodes.Status400BadRequest));
        }

        // On récupère l'événement à supprimer
        var eventToDelete =
            await _context.Events.FirstOrDefaultAsync(e =>
                e.Group.OwnerId == userContext.UserId && e.EventId == eventIdParsed);
        if (eventToDelete == null)
        {
            return NotFound(new ErrorMessage("No event found for this id", StatusCodes.Status404NotFound));
        }

        // On le supprime de la base de données
        _context.Events.Remove(eventToDelete);
        await _context.SaveChangesAsync();

        return Ok(new EventDto(eventToDelete));
    }

    /// <summary>
    /// Créer un événement
    /// </summary>
    /// <param name="groupId">L'id de l'événement</param>
    /// <param name="title">Le titre du l'événement</param>
    /// <param name="description">La description de l'événement</param>
    /// <param name="labels">Les labels utilisé pour l'événement</param>
    /// <param name="sessionDate">La date de session de l'événement</param>
    [HttpPost("create", Name = "Créer un événement")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EventDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> CreateEvent([FromForm] string groupId, [FromForm] string title,
        [FromForm] string description, [FromForm] string labels, [FromForm] string sessionDate)
    {
        // Si l'id du groupe est null, on retourne une erreur 400
        if (string.IsNullOrEmpty(groupId))
        {
            return BadRequest(new ErrorMessage("Group id not specified", StatusCodes.Status400BadRequest));
        }

        // On parse l'id du groupe de string à int
        var isGroupIdParsed = Int32.TryParse(groupId, out int groupIdParsed);
        if (!isGroupIdParsed)
        {
            return BadRequest(new ErrorMessage("Group id is not valid", StatusCodes.Status400BadRequest));
        }

        // On charge l'utilisateur
        var userContext = (User) HttpContext.Items["User"];
        if (userContext == null)
        {
            return Unauthorized(new ErrorMessage("Can't create a event", StatusCodes.Status401Unauthorized));
        }

        // On vérifie si le titre est null ou vide 
        if (string.IsNullOrEmpty(title))
        {
            return BadRequest(new ErrorMessage("Title not specified", StatusCodes.Status400BadRequest));
        }

        // On vérifie si la longueur du titre est supérieur à 25, si oui on retourne une erreur 400
        if (title.Length > 25)
        {
            return BadRequest(new ErrorMessage("Title is too long", StatusCodes.Status400BadRequest));
        }

        // On vérifie si la description est null ou vide.
        if (string.IsNullOrEmpty(description))
        {
            return BadRequest(new ErrorMessage("Description not specified", StatusCodes.Status400BadRequest));
        }

        // On vérifie si la longueur de la description est supérieur à 250, si oui on retourne une erreur 400
        if (description.Length > 250)
        {
            return BadRequest(new ErrorMessage("Description is too long", StatusCodes.Status400BadRequest));
        }

        // On vérifie si un label a été spécifié
        if (string.IsNullOrEmpty(labels))
        {
            return BadRequest(new ErrorMessage("Labels not specified, atleast specified one.",
                StatusCodes.Status400BadRequest));
        }

        // On vérifie si la date de session est null ou vide
        if (string.IsNullOrEmpty(sessionDate))
        {
            return BadRequest(new ErrorMessage("Session date not specified", StatusCodes.Status400BadRequest));
        }

        // On essaye de parser la date de session
        DateTime.TryParse(sessionDate, out var sessionDateParsed);
        var createEvent = new Event(sessionDateParsed, title, description);

        // On sépare les labels
        var labelsSplitted = labels.Split(',');

        // Si plus de 5 labels ont été spécifié, on retourne une erreur 400
        if (labelsSplitted.Length > 5)
        {
            return BadRequest(new ErrorMessage("Too many labels", StatusCodes.Status400BadRequest));
        }

        // S'il y en a plus que 0
        if (labelsSplitted.Length > 0)
        {
            createEvent.Labels = new List<Label>();

            // On ajoute chaque label à l'événement
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

        // On récupère le groupe qui a été spécifié
        var groupDb =
            _context.Groups.FirstOrDefault(g => g.GroupId == groupIdParsed && g.OwnerId == userContext.UserId);
        if (groupDb == null)
        {
            return BadRequest(new ErrorMessage("Group not found or the group owner is not the current user",
                StatusCodes.Status400BadRequest));
        }

        // On ajoute l'événement au groupe
        _context.Events.Add(createEvent);
        createEvent.Group = groupDb;

        await _context.SaveChangesAsync();

        return Ok(new EventDto(createEvent));
    }

    /// <summary>
    /// Met à jour un événement
    /// </summary>
    /// <param name="eventId">L'id de l'événement</param>
    /// <param name="title">Le titre de l'événement</param>
    /// <param name="description">Description de l'événement</param>
    /// <param name="labels">Les labels à remettre à jour</param>
    /// <returns></returns>
    [HttpPut("update/{eventId}", Name = "Met à jour un événement")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EventDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    public async Task<ActionResult> UpdateEvent(string eventId, [FromForm] string title, [FromForm] string description,
        [FromForm] string labels)
    {
        // On vérifie si l'id de l'événement est null ou vide
        if (string.IsNullOrEmpty(eventId))
        {
            return BadRequest(new ErrorMessage("Event id not specified", StatusCodes.Status400BadRequest));
        }

        // On charge l'utilisateur
        var userContext = (User) HttpContext.Items["User"];
        if (userContext == null)
        {
            return Unauthorized(new ErrorMessage("Can't create a event", StatusCodes.Status401Unauthorized));
        }

        // On parse l'id de l'événement de string à int
        bool isParsed = int.TryParse(eventId, out int eventIdParsed);
        if (!isParsed)
        {
            return BadRequest(new ErrorMessage("Event id is not valid", StatusCodes.Status400BadRequest));
        }

        // Si le titre est null ou vide, on retourne une erreur 400
        if (string.IsNullOrEmpty(title))
        {
            return BadRequest(new ErrorMessage("Title not specified", StatusCodes.Status400BadRequest));
        }

        // On vérifie que le titre ne soit pas plus grand que 25 caractères
        if (title.Length > 25)
        {
            return BadRequest(new ErrorMessage("Title is too long", StatusCodes.Status400BadRequest));
        }

        // On vérifie que la description ne soit pas null ou vide
        if (string.IsNullOrEmpty(description))
        {
            return BadRequest(new ErrorMessage("Description not specified", StatusCodes.Status400BadRequest));
        }

        // On vérifie que la description ne soit pas plus grande que 250 caractères
        if (description.Length > 250)
        {
            return BadRequest(new ErrorMessage("Description is too long", StatusCodes.Status400BadRequest));
        }

        // On vérifie que les labels ne soient pas null ou vide
        if (string.IsNullOrEmpty(labels))
        {
            return BadRequest(new ErrorMessage("Labels not specified", StatusCodes.Status400BadRequest));
        }

        // On récupère l'événement
        var eventToUpdate =
            await _context.Events.FirstOrDefaultAsync(e =>
                e.EventId == eventIdParsed && e.Group.OwnerId == userContext.UserId);
        if (eventToUpdate == null)
        {
            return NotFound(new ErrorMessage("Event not found", StatusCodes.Status404NotFound));
        }

        // On ajout les labels à la requête de récupération de l'événement
        _context.Entry(eventToUpdate).Collection(e => e.Labels).Load();

        // On sépare les labels
        var labelsSplitted = labels.Split(',');
        if (eventToUpdate.Labels.Count > 0)
        {
            var arrayOfIds = eventToUpdate.Labels.Select(l => l.LabelId).ToArray();
            labelsSplitted = labelsSplitted.Where(l => !arrayOfIds.Contains(Int32.Parse(l))).ToArray();
        }

        // On vérifie que le nombre de labels ne soit pas supérieur à 5
        if (labelsSplitted.Length > 5)
        {
            return BadRequest(new ErrorMessage("Too many labels", StatusCodes.Status400BadRequest));
        }

        // Si on a plus de 0 labels
        if (labelsSplitted.Length > 0)
        {
            eventToUpdate.Labels = new List<Label>();
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

                eventToUpdate.Labels.Add(labelDb);
            }
        }

        // On modifie le titre de l'événement et la description
        eventToUpdate.EventTitle = title;
        eventToUpdate.EventDescription = description;

        await _context.SaveChangesAsync();

        return Ok(new EventDto(eventToUpdate));
    }
}