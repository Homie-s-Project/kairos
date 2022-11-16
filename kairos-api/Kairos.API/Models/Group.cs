using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kairos.API.Models;

public class Group
{
    public Group()
    {
    }

    public Group(string groupName, int userId, bool groupsIsPrivate = false)
    {
        GroupName = groupName;
        UserId = userId;
        GroupsIsPrivate = groupsIsPrivate;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int GroupId { get; set; }

    public string GroupName { get; set; }
    public bool GroupsIsPrivate { get; set; }

    public int UserId { get; set; }

    [ForeignKey("Event")] public int? EventId { get; set; }
    public virtual Event Event { get; set; }

    public virtual ICollection<User> Users { get; set; }
    public ICollection<Label> Labels { get; set; }
}