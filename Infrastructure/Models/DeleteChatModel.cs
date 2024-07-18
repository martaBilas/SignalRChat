using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models;

public class DeleteChatModel
{
    [Required]
    public int ChatId { get; set; }
    [Required]
    public int CreatorId { get; set; }
}
