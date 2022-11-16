using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kairos.API.Models;

public class Reminder
{
    public Reminder(int reminderId, DateTime reminderTime, int studiesId)
    {
        ReminderId = reminderId;
        ReminderTime = reminderTime;
        StudiesId = studiesId;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ReminderId { get; set; }

    public DateTime ReminderTime { get; set; }
    [ForeignKey("Studies")] public int StudiesId { get; set; }
    public virtual Studies Studies { get; set; }
}