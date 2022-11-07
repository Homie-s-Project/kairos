using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kairos.API.Models;

public class Group
{
    public Group(int groupId, string groupName, DateTime endDate, int eventId)
    {
        GroupId = groupId;
        GroupName = groupName;
        EndDate = endDate;
        EventId = eventId;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int GroupId { get; set; }
    public string GroupName { get; set; }
    public DateTime EndDate { get; set; }
    [ForeignKey("Event")] public int EventId { get; set; }
    public virtual Event Event { get; set; }
    public ICollection<User> Users { get; set; }
}