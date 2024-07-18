using Infrastructure.Models;

namespace Infrastructure.Interfaces
{
    public interface IChatService
    {
        void AddUsersToChat(ICollection<int> usersToAddIds, int chatId);
        void CreateNewChat(int creatorId, string roomName);
        bool IsChatExist(int chatId);
        IList<ChatModel> SearchChatsByName(int userId, string searchTerm);
        void SendMessageToChat(int userId, int chatId, string message);
    }
}