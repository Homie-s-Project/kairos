using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kairos.API.Models;

public class Label
{
    public Label(int labelId, string labelTitle, int userId)
    {
        LabelId = labelId;
        LabelTitle = labelTitle;
        UserId = userId;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int LabelId { get; set; }
    public string LabelTitle { get; set; }
    [ForeignKey("User")] public int UserId { get; set; }
    public virtual User User { get; set; }
}