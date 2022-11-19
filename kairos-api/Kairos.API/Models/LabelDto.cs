using System.Collections.Generic;

namespace Kairos.API.Models;

public class LabelDto
{
    public LabelDto(Label label)
    {
        LabelId = label.LabelId;
        labelTitle = label.LabelTitle;
        User = new UserDTO(label.User);
        Groups = label.Groups;
        Events = label.Events;
        Studies = label.Studies;
    }

    public int LabelId { get; set; }
    public string labelTitle { get; set; }
    public UserDTO User { get; set; }
    public ICollection<Group> Groups { get; set; }
    public ICollection<Event> Events { get; set; }
    public ICollection<Studies> Studies { get; set; }
}