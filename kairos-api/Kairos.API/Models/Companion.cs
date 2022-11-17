using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kairos.API.Models;

public class Companion
{
    // Par défaut, si aucun âge n'est donné, il aura 1 an et ce sera un chat.
    public Companion(int companionId, string companionName, int userId, CompanionType companionType = CompanionType.CAT,
        int companionAge = 1)
    {
        CompanionId = companionId;
        CompanionName = companionName;
        CompanionType = companionType;
        CompanionAge = companionAge;
        UserId = userId;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CompanionId { get; set; }

    public string CompanionName { get; set; }
    public CompanionType CompanionType { get; set; }
    public int CompanionAge { get; set; }

    public ICollection<Item> Items { get; set; }

    [ForeignKey("User")] public int UserId { get; set; }
    public virtual User User { get; set; }
}

public enum CompanionType
{
    DOG,
    CAT,
    SNAKE,
    COW
}