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

    [MaxLength(50)] public string LabelTitle { get; set; }

    [ForeignKey("User")] public int UserId { get; set; }
    public virtual User User { get; set; }

    public ICollection<Group> Groups { get; set; }
    public ICollection<Event> Events { get; set; }
    public ICollection<Studies> Studies { get; set; }
}