using System;
using System.Collections.Generic;
using System.Linq;

namespace Kairos.API.Models;

public class EventDto
{

    public EventDto(Event eventDb)
    {
        EventId = eventDb.EventId;
        EventDate = eventDb.EventDate;
        EventTitle = eventDb.EventTitle;
        EventDescription = eventDb.EventDescription;
        EventCreatedDate = eventDb.EventCreatedDate;

        if (eventDb.Labels != null)
        {
            Labels = eventDb.Labels.Select(l => new LabelDto(l, false)).ToList();
        }

        if (eventDb.Group != null)
        {
            Group = new GroupDto(eventDb.Group);
        }
    } 
    
    public EventDto(Event eventDb, bool loadMore = true)
    {
        EventId = eventDb.EventId;
        EventDate = eventDb.EventDate;
        EventTitle = eventDb.EventTitle;
        EventDescription = eventDb.EventDescription;
        EventCreatedDate = eventDb.EventCreatedDate;

        if (loadMore)
        {
            Labels = eventDb.Labels.Select(l => new LabelDto(l, false)).ToList();

            if (eventDb.Group != null)
            {
                Group = new GroupDto(eventDb.Group);
            }
        }
    } 

    public int EventId { get; set; }
    public DateTime EventDate { get; set; }
    public string EventTitle { get; set; }
    public string EventDescription { get; set; }
    public DateTime EventCreatedDate { get; set; }
    public GroupDto Group { get; set; }
    public List<LabelDto> Labels { get; set; }
}