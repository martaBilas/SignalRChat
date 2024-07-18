using Domain;
using Infrastructure.Exceptions;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace ChatTest.Hubs;

public class ChatHub : Hub
{
    private static readonly ConcurrentDictionary<string, string> _userGroups = new ConcurrentDictionary<string, string>();
    public async Task JoinGroup(int userId, int chatId)
    {
        var serviceProvide = Context.GetHttpContext().RequestServices;
        var userService = serviceProvide.GetRequiredService<IUserService>();

        if (!userService.IsUserInChat(userId, chatId)) {
            throw new IsNotChatMemberException();
        }
        _userGroups[Context.ConnectionId] = chatId.ToString();
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", $"{Context.ConnectionId} has joined the group {chatId.ToString()}.");

    }
    public async Task SendMessageToGroup(int chatId, int userId, string message)
    {
        var serviceProvide = Context.GetHttpContext().RequestServices;
        var userService = serviceProvide.GetRequiredService<IUserService>();
        var chatService = serviceProvide.GetRequiredService<IChatService>();

        var user =  userService.GetUserById(userId);

        if (_userGroups.TryGetValue(Context.ConnectionId, out var userGroups) &&
          userGroups.Contains(chatId.ToString()))
        {
            chatService.SendMessageToChat(chatId, userId, message);
            await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", $"{user.Name}: {message}");
        }
        else
        {
            throw new IsNotChatMemberException();
        }
    }
}
