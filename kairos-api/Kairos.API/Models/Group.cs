using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kairos.API.Models;

public class Group
{
    public Group(string groupName, int userId, bool groupsIsPrivate = false)
    {
        GroupName = groupName;
        UserId = userId;
        GroupsIsPrivate = groupsIsPrivate;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int GroupId { get; set; }

    [MaxLength(50)] public string GroupName { get; set; }
    public bool GroupsIsPrivate { get; set; }

    public int UserId { get; set; }

    [ForeignKey("Event")] public int? EventId { get; set; }
    public Event Event { get; set; }

    public ICollection<User> Users { get; set; }
    public ICollection<Label> Labels { get; set; }
}