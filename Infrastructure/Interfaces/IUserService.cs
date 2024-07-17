namespace Infrastructure.Interfaces
{
    public interface IUserService
    {
        bool CheckIsUserCreator(int userId, int chatId);
        bool IsUserExist(int userId);
        bool IsUserInChat(int userId, int chatId);
    }
}