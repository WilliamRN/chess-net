using ChessNet.Data.Enums;

namespace ChessNet.Data.Constants
{
    public static class GameEndStates
    {
        public static List<GameStates> LIST = new List<GameStates>()
        {
            GameStates.CheckMate,
            GameStates.End,
            GameStates.Surrender,
        };
    }
}