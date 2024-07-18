using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models;

public class CreateChatModel
{
    [Required(ErrorMessage = "CreatorId is required.")]
    public int CreatorId { get; set; }

    [Required(ErrorMessage = "Chat name is required.")]
    public string Name { get; set; }
}
