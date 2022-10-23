using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kairos.API.Models;

public class User
{
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserId { get; set; }
    public string MicrosoftId { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public DateTime BirthDate { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }

    public User(string microsoftId, string lastName, string firstName, DateTime birthDate, string email, DateTime lastUpdatedAt)
    {
        MicrosoftId = microsoftId;
        LastName = lastName;
        FirstName = firstName;
        BirthDate = birthDate;
        Email = email;
        CreatedAt = DateTime.UtcNow;
        LastUpdatedAt = lastUpdatedAt;
    }
}