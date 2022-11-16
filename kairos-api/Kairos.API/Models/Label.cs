using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kairos.API.Models;

public class Label
{
    public Label(string labelTitle, int userId)
    {
        LabelTitle = labelTitle;
        UserId = userId;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int LabelId { get; set; }

    public string LabelTitle { get; set; }
    [ForeignKey("User")] public int UserId { get; set; }
    public virtual User User { get; set; }

    public virtual ICollection<Group> Groups { get; set; }
    public virtual ICollection<Event> Events { get; set; }
    public virtual ICollection<Studies> Studies { get; set; }
}