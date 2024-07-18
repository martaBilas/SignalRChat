using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models;

public class AddUsersToChatModel
{
    [Required]
    public int CreatorId { get; set; }
    [Required]
    public ICollection<int> UsersToAddIds { get; set; }
    [Required]
    public int ChatId { get; set; }
}
