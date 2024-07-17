namespace Infrastructure.Exceptions;
public class UserIsAddedException : Exception
{
    public UserIsAddedException() : base("User is already added to this chat.") { }
    public UserIsAddedException(string message) : base(message) { }
    public UserIsAddedException(string message, Exception innerException) : base(message, innerException) { }
}