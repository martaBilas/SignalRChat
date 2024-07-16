using DataContext;
using Domain;
using Infrastructure.Interfaces;

namespace Infrastructure.Services;

public class ChatService : IChatService
{
    private readonly ChatDataContext _db;
    public ChatService(ChatDataContext db)
    { _db = db; }
    public void CreateRoom(int creatorId, string roomName)
    {
        var newChat = new Chat { UserCreatorId = creatorId, Name = roomName };
        _db.Chats.Add(newChat);
        _db.SaveChanges();
    }
    public List<Chat> SearchChatsByName(int userId, string searchTerm)
    {
        var chats = _db.Chats
                  .Where(c => c.Users.Any(u => u.Id == userId) && c.Name.Contains(searchTerm))
                  .ToList();

        return chats;
    }
}
