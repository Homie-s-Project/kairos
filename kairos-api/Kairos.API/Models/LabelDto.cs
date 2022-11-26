using System.Collections.Generic;
using System.Linq;

namespace Kairos.API.Models;

public class LabelDto
{
    public LabelDto(Label label)
    {
        LabelId = label.LabelId;
        LabelTitle = label.LabelTitle;
        User = new UserDto(label.User);
        Groups = label.Groups.Select(g => new GroupDto(g)).ToList();
        Events = label.Events.Select(e => new EventDto(e, false)).ToList();
        Studies = label.Studies.Select(s => new StudiesDto(s, false)).ToList();
    }
    
    public LabelDto(Label label, bool loadMore = false, bool loadUser = false)
    {
        LabelId = label.LabelId;
        LabelTitle = label.LabelTitle;
        
        if (loadUser) {
            User = new UserDto(label.User);
        }

        if (loadMore)
        {
            Groups = label.Groups.Select(g => new GroupDto(g)).ToList();
            Events = label.Events.Select(e => new EventDto(e, false)).ToList();
            Studies = label.Studies.Select(s => new StudiesDto(s, false)).ToList();
        }
    }

    public int LabelId { get; set; }
    public string LabelTitle { get; set; }
    public UserDto User { get; set; }
    public List<GroupDto> Groups { get; set; }
    public List<EventDto> Events { get; set; }
    public List<StudiesDto> Studies { get; set; }
}