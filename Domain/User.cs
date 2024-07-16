namespace Domain;

public class User
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public ICollection<Chat> UserChats { get; set; }
}
