using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models;

public class SearchChatByNameModel
{
    [Required(ErrorMessage = "UserId is required.")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Chat name is required.")]
    public string Name { get; set; }
}
