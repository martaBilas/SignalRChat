using DataContext;
using Domain;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class UserService : IUserService
{
    private readonly ChatDataContext _db;
    public UserService(ChatDataContext db)
    { _db = db; }

    public bool CheckIsUserCreator(int userId, int chatId)
    {
        var chat = _db.Chats.FirstOrDefault(c => c.Id == chatId && c.UserCreatorId == userId);
        return chat != null && chat.Id > 0;
    }

    public bool IsUserExist(int userId)
    {
        var user= _db.Users.FirstOrDefault(c => c.Id == userId);
        return user != null && user.Id > 0;
    }
    public bool IsUserInChat(int userId, int chatId)
    {
        return _db.UsersChats.Any(uc => uc.UserId == userId && uc.ChatId == chatId);
        
    }

}
