using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kairos.API.Models;

public class Studies
{
    public Studies(int studiesId, DateTime studiesDate, string studiesNumber, string studiesTime,
        StudiesOwnerType ownerType, int ownerId)
    {
        StudiesId = studiesId;
        StudiesDate = studiesDate;
        StudiesNumber = studiesNumber;
        StudiesTime = studiesTime;
        OwnerType = ownerType;
        OwnerId = ownerId;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int StudiesId { get; set; }

    public DateTime StudiesDate { get; set; }

    // UUID
    public string StudiesNumber { get; set; }

    // EN ms (1sec. = 1000ms)
    public string StudiesTime { get; set; }
    public StudiesOwnerType OwnerType { get; set; }
    public int OwnerId { get; set; }
}

public enum StudiesOwnerType
{
    User,
    Group
}