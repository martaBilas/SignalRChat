namespace Infrastructure.Models;

public class AddUserToChatModel
{
    public int CreatorId { get; set; }
    public ICollection<int> UsersToAddIds { get; set; }
    public int ChatId { get; set; }
}
