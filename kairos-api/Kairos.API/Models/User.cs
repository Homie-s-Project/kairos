using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kairos.API.Models;

public class User
{
    public User(string serviceId, string lastName, string firstName, DateTime birthDate, string email,
        DateTime lastUpdatedAt)
    {
        ServiceId = serviceId;
        LastName = lastName;
        FirstName = firstName;
        BirthDate = birthDate;
        Email = email;
        CreatedAt = DateTime.UtcNow;
        LastUpdatedAt = lastUpdatedAt;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserId { get; set; }

    [Required] public string ServiceId { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public DateTime BirthDate { get; set; }
    [Required] public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    public virtual ICollection<Group> Groups { get; set; }
}