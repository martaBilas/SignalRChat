namespace Infrastructure.Models;

public class MessageModel
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public UserModel? User { get; set; }

}
