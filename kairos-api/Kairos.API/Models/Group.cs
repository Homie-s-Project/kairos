using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kairos.API.Models;

public class Group
{
    public Group(string groupName, int ownerId, bool groupsIsPrivate = false)
    {
        GroupName = groupName;
        OwnerId = ownerId;
        GroupsIsPrivate = groupsIsPrivate;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int GroupId { get; set; }

    [MaxLength(50)] public string GroupName { get; set; }
    public bool GroupsIsPrivate { get; set; }
    public int OwnerId { get; set; }
    public ICollection<User> Users { get; set; }
    public ICollection<Label> Labels { get; set; }
    public ICollection<Event> Events { get; set; }
}