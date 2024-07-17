using DataContext;
using Domain;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure.Services;

public class ChatService : IChatService
{
    private readonly ChatDataContext _db;
    public ChatService(ChatDataContext db)
    { _db = db; }
    public void CreateNewChat(int creatorId, string roomName)
    {
        var newChat = new Chat { UserCreatorId = creatorId, Name = roomName };
        _db.Chats.Add(newChat);
        _db.SaveChanges();

        var userChat = new UserChat { UserId = creatorId, ChatId = newChat.Id };
        _db.UsersChats.Add(userChat);
        _db.SaveChanges();
    }
    public IList<ChatModel> SearchChatsByName(int userId, string searchTerm)
    {
        var chatsDB = _db.Chats
            .Include(c => c.Users)
            .Include(c => c.UserCreator)
            .Where(c => c.Users.Any(u => u.Id == userId) && c.Name.Contains(searchTerm))
            .ToList();

        IList<ChatModel> chats = chatsDB.Select(chat => new ChatModel
        {
            Id = chat.Id,
            Name = chat.Name,
            Members = chat.Users.Select(member => new UserModel
            {
                Id = member.Id,
                Name = member.UserName
            }),
            Creator = new UserModel
            {
                Id=chat.UserCreator.Id,
                Name = chat.UserCreator.UserName
            },
            Messages = chat.Messages?.Select(message => new MessageModel
            {
                Id = message.Id,
                Content = message.Content,
                User =
                {
                    Id=message.UserId,
                    Name=message.User.UserName
                }
            })
        }).ToList();

        return chats;
    }

    public void AddUsersToChat(ICollection<int> usersToAddIds, int chatId)
    {
        foreach (var userId in usersToAddIds)
        {
            var userChat = new UserChat
            {
                UserId = userId,
                ChatId = chatId
            };

            _db.UsersChats.Add(userChat);
            _db.SaveChanges();
        }
    }

    public void DeleteChat(int chatId) 
    {
        var chat = _db.Chats.FirstOrDefault(c => c.Id == chatId);
        _db.Chats.Remove(chat);
    }

    public bool IsChatExist(int chatId)
    {
        var chat = _db.Chats.FirstOrDefault(c => c.Id == chatId);
        return chat != null && chat.Id > 0;
    }
}
