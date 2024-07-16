namespace Domain;

public class Chat
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public ICollection<User> Users { get; set; }
    public ICollection<Message> Messages { get; set; }
    public User UserCreator { get; set; }
    public int UserCreatorId { get; set; }
}
