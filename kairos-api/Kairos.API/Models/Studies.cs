using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kairos.API.Models;

public class Studies
{
    public Studies(int studiesId, string studiesNumber, string studiesTime)
    {
        StudiesId = studiesId;
        StudiesNumber = studiesNumber;
        StudiesTime = studiesTime;
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