namespace Infrastructure.Exceptions;
public class IsNotChatMemberException : Exception
{
    public IsNotChatMemberException() : base("User is not a member of this chat.") { }
    public IsNotChatMemberException(string message) : base(message) { }
    public IsNotChatMemberException(string message, Exception innerException) : base(message, innerException) { }
}