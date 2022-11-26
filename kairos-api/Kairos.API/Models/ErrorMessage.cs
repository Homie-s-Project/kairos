namespace Kairos.API.Models;

public class ErrorMessage
{
    public ErrorMessage(string message, int statusCode)
    {
        Message = message;
        StatuCode = statusCode;
    }
    
    public string Message {get; set;}
    public int StatuCode {get; set;}
}