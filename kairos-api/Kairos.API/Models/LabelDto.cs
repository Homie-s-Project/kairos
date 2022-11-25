using System.Collections.Generic;
using System.Linq;

namespace Kairos.API.Models;

public class LabelDto
{
    public LabelDto(Label label)
    {
        LabelId = label.LabelId;
        labelTitle = label.LabelTitle;
        User = new UserDto(label.User);
        Groups = label.Groups.Select(g => new GroupDto(g)).ToList();
        Events = label.Events.Select(e => new EventDto(e)).ToList();
        Studies = label.Studies.Select(s => new StudiesDto(s)).ToList();
    }

    public int LabelId { get; set; }
    public string labelTitle { get; set; }
    public UserDto User { get; set; }
    public List<GroupDto> Groups { get; set; }
    public List<EventDto> Events { get; set; }
    public List<StudiesDto> Studies { get; set; }
}