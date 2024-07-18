using Infrastructure.Models;

namespace Infrastructure.Interfaces
{
    public interface IUserService
    {
        bool CheckIsUserCreator(int userId, int chatId);
        UserModel GetUserById(int userId);
        bool IsUserExist(int userId);
        bool IsUserInChat(int userId, int chatId);
    }
}