using Infrastructure.Exceptions;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;


namespace SignalRChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly IUserService _userService;

        public ChatsController(IChatService chatService, IUserService userService)
        {
            _chatService = chatService;
            _userService = userService;
        }

        [HttpGet("SearchChatsByName")]
        public IActionResult SearchChatsByName([FromQuery] int userId, [FromQuery] string chatName)
        {
            if (!_userService.IsUserExist(userId))
            {
                return NotFound($"User with ID {userId} not found.");
            }
            var chats = _chatService.SearchChatsByName(userId, chatName);
            if (chats == null || !chats.Any())
            {
                return NotFound(new { Message = "No chats found for the given search criteria." });
            }
            return Ok(chats);
        }


        [HttpPost("CreateNewChat")]
        public IActionResult CreateChat([FromBody] CreateChatModel newChatModel)
        {

            if (!_userService.IsUserExist(newChatModel.CreatorId))
            {
                return NotFound($"User with ID {newChatModel.CreatorId} not found.");
            }

            _chatService.CreateNewChat(newChatModel.CreatorId, newChatModel.Name);
            return Ok("Chat is created");

        }

        [HttpPost("AddUsersToChat")]
        public IActionResult AddUsersToChat(AddUsersToChatModel model)
        {
            if (!_chatService.IsChatExist(model.ChatId))
                return NotFound(new { Message = "There is no chat with such id" });

            if (!_userService.CheckIsUserCreator(model.CreatorId, model.ChatId))
                throw new NotCreatorException();

            foreach (var userId in model.UsersToAddIds)
            {
                if (!_userService.IsUserExist(userId))
                {
                    return NotFound($"User with ID {userId} not found.");
                }
            }

            foreach (var userId in model.UsersToAddIds)
            {
                if (_userService.IsUserInChat(userId, model.ChatId))
                {
                    throw new UserIsAddedException($"User with ID {userId} is already added to this chat.");
                }
            }

            _chatService.AddUsersToChat(model.UsersToAddIds, model.ChatId);
            return Ok("Users added to chat");

        }

        [HttpDelete("DeleteChat")]
        public IActionResult DeleteChat(DeleteChatModel model)
        {
            if (!_chatService.IsChatExist(model.ChatId))
                throw new IsNotChatMemberException();

            if (!_userService.CheckIsUserCreator(model.CreatorId, model.ChatId))
                throw new NotCreatorException();

            _chatService.DeleteChat(model.ChatId);
            return Ok("Chat is deleted");
        }
    }
}
