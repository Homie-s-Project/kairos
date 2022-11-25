using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kairos.API.Models;

public class Studies
{
    public Studies(string studiesNumber, string studiesTime, DateTime studiesCreatedDate)
    {
        StudiesNumber = studiesNumber;
        StudiesTime = studiesTime;
        StudiesCreatedDate = studiesCreatedDate;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int StudiesId { get; set; }

    public string StudiesNumber { get; set; }
    public string StudiesTime { get; set; }
    public DateTime StudiesCreatedDate { get; set; }

    public ICollection<Label> Labels { get; set; }

    [ForeignKey("Group")] public int GroupId { get; set; }
    public virtual Group Group { get; set; }
}