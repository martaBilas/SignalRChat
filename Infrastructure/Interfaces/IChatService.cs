namespace Infrastructure.Interfaces
{
    public interface IChatService
    {
        void CreateRoom(int creatorId, string roomName);
    }
}