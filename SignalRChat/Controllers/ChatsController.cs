using ChatTest.Hubs;
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

        public ChatsController(IHubContext<ChatHub> hubContext, IChatService chatService)
        {
            _hubContext = hubContext;
            _chatService = chatService;
        }

        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] MessageModel model)
        {
            if (string.IsNullOrEmpty(model.Message))
            {
                return BadRequest("Invalid message.");
            }
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", model.User, model.Message);

            return Ok();
        }

        // GET api/<ChatsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }


        [HttpPost("CreateChat")]
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
            try
            {
                _chatService.CreateRoom(newChatModel.CreatorId, newChatModel.Name);
                return Ok("Chat is created");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while creating the chat room.", Details = ex.Message });
            }
        }

            // PUT api/<ChatsController>/5
            [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

   
    }
}
