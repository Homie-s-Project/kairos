using System.Collections.Generic;
using System.Linq;

namespace Kairos.API.Models;

public class GroupDto
{
    public GroupDto(int groupId, string groupName, bool groupsIsPrivate)
    {
        GroupId = groupId;
        GroupName = groupName;
        GroupsIsPrivate = groupsIsPrivate;
    }

    public GroupDto(Group group)
    {
        GroupId = group.GroupId;
        GroupName = group.GroupName;
        GroupsIsPrivate = group.GroupsIsPrivate;
    }
    
    public GroupDto(Group group, bool loadMore = true, bool loadAllUsers = false)
    {
        GroupId = group.GroupId;
        GroupName = group.GroupName;
        GroupsIsPrivate = group.GroupsIsPrivate;
        
        if (loadMore)
        {
            if (group.Event != null)
            {
                Event = new EventDto(group.Event);
            }

            if (group.Labels != null)
            {
                Labels = group.Labels.Select(l => new LabelDto(l, false)).ToList();
            }
        }

        if (loadAllUsers)
        {
            Users = group.Users.Select(u => new UserDto(u)).ToList();
        }
    }

    public int GroupId { get; set; }
    public string GroupName { get; set; }
    public bool GroupsIsPrivate { get; set; }
    public EventDto Event { get; set; }
    public List<LabelDto> Labels { get; set; }
    public List<UserDto> Users { get; set; }
}