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

    public int GroupId { get; set; }
    public string GroupName { get; set; }
    public bool GroupsIsPrivate { get; set; }
}