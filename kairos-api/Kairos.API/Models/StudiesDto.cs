using System;
using System.Collections.Generic;
using System.Linq;

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
        StudiesLabels = studiesLabels.Select(l => new LabelDto(l)).ToList();
        Group = new GroupDto(group);
    }

    public StudiesDto(Studies studies)
    {
        StudiesId = studies.StudiesId;
        StudiesNumber = studies.StudiesNumber;
        StudiesTime = studies.StudiesTime;
        StudiesCreatedDate = studies.StudiesCreatedDate;
        StudiesLabels = studies.Labels.Select(l => new LabelDto(l)).ToList();
        Group = new GroupDto(studies.Group);
    }
    
    public StudiesDto(Studies studies, bool loadMore = true)
    {
        StudiesId = studies.StudiesId;
        StudiesNumber = studies.StudiesNumber;
        StudiesTime = studies.StudiesTime;
        StudiesCreatedDate = studies.StudiesCreatedDate;
        
        if (loadMore)
        {
            Group = new GroupDto(studies.Group);
            StudiesLabels = studies.Labels.Select(l => new LabelDto(l)).ToList();
        }
    }

    public int StudiesId { get; set; }
    public string StudiesNumber { get; set; }
    public string StudiesTime { get; set; }
    public DateTime StudiesCreatedDate { get; set; }
    public List<LabelDto> StudiesLabels { get; set; }
    public GroupDto Group { get; set; }
}