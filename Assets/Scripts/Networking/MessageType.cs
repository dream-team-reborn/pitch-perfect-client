namespace PitchPerfect.Networking
{
    public enum MessageType
    {
        CreateRoom,
        GetRooms,
        JoinRoom,
        PlayerReady,
        GameStarted,
        TurnStarted,
        RoomJoined,
        PlayerCardsSelected,
        AllPlayerSelectedCards,
        PlayerRatedOtherCards,
        LeaveRoom,
        Lapis
    }
}