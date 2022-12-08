using System;
using System.Collections.Generic;

namespace Kairos.API.Models;

public class LiveStudies
{
   public LiveStudies(int userId, int timePlanned, List<LabelDto> labels)
   {
      UserId = userId;
      TimePlanned = timePlanned;
      Labels = labels;
      StartTime = DateTime.UtcNow;
      LastRefresh = DateTime.UtcNow;
   }

   public LiveStudies()
   {
      StartTime = DateTime.UtcNow;
      LastRefresh = DateTime.UtcNow;
   }

   public int UserId {get; set; }
   public int TimePlanned { get; set; }
   public List<LabelDto> Labels { get; set; }
   public DateTime StartTime { get; set; }
   public DateTime LastRefresh { get; set; }
}