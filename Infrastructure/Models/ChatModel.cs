namespace Infrastructure.Models;

public class ChatModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public IEnumerable<UserModel>? Members { get; set; }
    public UserModel Creator { get; set; }
    public IEnumerable<MessageModel>? Messages { get; set; }

}
