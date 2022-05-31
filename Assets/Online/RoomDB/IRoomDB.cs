namespace Online.RoomDB
{
    public interface IRoomDB
    {
        void SaveRoom(string txt);
        string LoadRoom();
    }
}