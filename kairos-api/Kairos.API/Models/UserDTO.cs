namespace Kairos.API.Models;

public class UserDTO
{
    public UserDTO(int userId, string lastName, string firstName, string email)
    {
        UserId = userId;
        LastName = lastName;
        FirstName = firstName;
        Email = email;
    }

    public UserDTO(User user)
    {
        UserId = user.UserId;
        LastName = user.LastName;
        FirstName = user.FirstName;
        Email = user.Email;
    }

    public int UserId { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Email { get; set; }
}