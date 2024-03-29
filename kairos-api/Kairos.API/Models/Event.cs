using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kairos.API.Models;

public class Event
{
    public Event(DateTime eventDate, string eventTitle, string eventDescription)
    {
        EventDate = eventDate;
        EventTitle = eventTitle;
        EventDescription = eventDescription;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EventId { get; set; }

    public DateTime EventDate { get; set; }
    [MaxLength(25)]
    public string EventTitle { get; set; }
    [MaxLength(250)]
    public string EventDescription { get; set; }
    public DateTime EventCreatedDate { get; set; }
    [ForeignKey("Group")] public int? GroupId { get; set; }
    public Group Group { get; set; }
    public ICollection<Label> Labels { get; set; }
}