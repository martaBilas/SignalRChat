using ChatTest.Hubs;
using Domain;
using Infrastructure.Exceptions;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;


namespace SignalRChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IChatService _chatService;
        private readonly IUserService _userService;

        public ChatsController(IHubContext<ChatHub> hubContext, IChatService chatService, IUserService userService)
        {
            _hubContext = hubContext;
            _chatService = chatService;
            _userService = userService;
        }

        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] MessageTestModel model)
        {
            if (string.IsNullOrEmpty(model.Message))
            {
                return BadRequest("Invalid message.");
            }
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", model.User, model.Message);

            return Ok();
        }
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        [HttpGet("SearchChatsByName")]
        public IActionResult SearchChatsByName([FromQuery] int userId, [FromQuery] string chatName)
        {
            try
            {
                var chats = _chatService.SearchChatsByName(userId, chatName);
                if (chats == null || !chats.Any())
                {
                    return NotFound(new { Message = "No chats found for the given search criteria." });
                }
                return Ok(chats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while searching chats.", Details = ex.Message });
            }
        }


        [HttpPost("CreateNewChat")]
        public IActionResult CreateChat([FromBody] CreateChatModel newChatModel)
        {
            //if (!ModelState.IsValid)
            //{
            //    var errors = ModelState.Values
            //        .SelectMany(v => v.Errors)
            //        .Select(e => e.ErrorMessage)
            //        .ToList();

            //    return BadRequest(new { Errors = errors });
            //}
            if (!_userService.IsUserExist(newChatModel.CreatorId))
            {
                return NotFound($"User with ID {newChatModel.CreatorId} not found.");
            }

            try
            {
                _chatService.CreateNewChat(newChatModel.CreatorId, newChatModel.Name);
                return Ok("Chat is created");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while creating the chat room.", Details = ex.Message });
            }
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

            try
            {
                _chatService.AddUsersToChat(model.UsersToAddIds, model.ChatId);
                return Ok("Users added to chat");
            }
            catch (Exception ex) { 
                return StatusCode(500, new { Error = "An error occurred while adding users to the chat room.", Details = ex.Message }); 
            }
        }

        [HttpDelete("DeleteChat")]
        public IActionResult DeleteChat(DeleteChatModel model) {
            if (!_chatService.IsChatExist(model.ChatId))
                return NotFound(new { Message = "There is no chat with such id" });

            if (!_userService.CheckIsUserCreator(model.CreatorId, model.ChatId))
                throw new NotCreatorException();
            try
            {
                return Ok("Chat is deleted");

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while deleting the chat room.", Details = ex.Message });
            }
        }
    }
}
