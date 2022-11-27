using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kairos.API.Models;

public class Item
{
    public Item(int itemId, string itemName, ItemType itemType)
    {
        ItemId = itemId;
        ItemName = itemName;
        ItemType = itemType;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ItemId { get; set; }

    public string ItemName { get; set; }
    public ItemType ItemType { get; set; }

    public ICollection<Companion> Companions { get; set; }
}

public enum ItemType
{
    DRINK,
    EAT,
    TOY
}