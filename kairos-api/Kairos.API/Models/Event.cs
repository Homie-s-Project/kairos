using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kairos.API.Models;

public class Event
{
    public Event(int eventId, DateTime eventDate, string eventTitle, string eventDescription, StudiesOwnerType ownerType, int ownerId)
    {
        EventId = eventId;
        EventDate = eventDate;
        EventTitle = eventTitle;
        EventDescription = eventDescription;
        OwnerType = ownerType;
        OwnerId = ownerId;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EventId { get; set; }
    public DateTime EventDate { get; set; }
    public string EventTitle { get; set; }
    public string EventDescription { get; set; }
    public StudiesOwnerType OwnerType { get; set; }
    public int OwnerId { get; set; }
    public ICollection<Label> Labels { get; set; }
}
