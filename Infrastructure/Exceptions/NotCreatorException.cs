namespace Infrastructure.Exceptions;

public class NotCreatorException : Exception
{
    public NotCreatorException() : base("User is not a creator of this chat.") { }
    public NotCreatorException(string message) : base(message) { }
    public NotCreatorException(string message, Exception innerException) : base(message, innerException) { }
}
