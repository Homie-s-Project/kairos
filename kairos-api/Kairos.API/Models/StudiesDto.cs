using System;
using System.Collections.Generic;

namespace Kairos.API.Models;

public class StudiesDto
{
    public StudiesDto(int studiesId, string studiesNumber, string studiesTime, DateTime studiesCreatedDate,
        List<Label> studiesLabels, Group group)
    {
        StudiesId = studiesId;
        StudiesNumber = studiesNumber;
        StudiesTime = studiesTime;
        StudiesCreatedDate = studiesCreatedDate;
        StudiesLabels = studiesLabels;
        Group = group;
    }

    public StudiesDto(Studies studies)
    {
        StudiesId = studies.StudiesId;
        StudiesNumber = studies.StudiesNumber;
        StudiesTime = studies.StudiesTime;
        StudiesCreatedDate = studies.StudiesCreatedDate;
        StudiesLabels = studies.Labels as List<Label>;
        Group = studies.Group;
    }

    public int StudiesId { get; set; }
    public string StudiesNumber { get; set; }
    public string StudiesTime { get; set; }
    public DateTime StudiesCreatedDate { get; set; }
    public List<Label> StudiesLabels { get; set; }
    public Group Group { get; set; }
}