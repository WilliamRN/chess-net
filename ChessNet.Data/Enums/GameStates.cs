using ChessNet.Data.Structs;

namespace ChessNet.Data.Enums
{
    public enum GameStates
    {
        Setup,
        Start,
        Playing,
        Check,
        CheckMate,
        Surrender,
        End,
        Error,
    }
}