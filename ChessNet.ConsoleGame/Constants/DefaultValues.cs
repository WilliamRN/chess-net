using ChessNet.ConsoleGame.Enums;

namespace ChessNet.ConsoleGame.Constants
{
    internal static class DefaultValues
    {
        internal const string PLAYER_1 = "Player One";
        internal const string PLAYER_2 = "Player Two";

        internal const int ACTION_DELAY = 100; //ms
        internal const int ACTION_DELAY_AI_ONLY = 50; //ms

        internal const GameplayMode GAMEPLAY_MODE = GameplayMode.AIOnly;
    }
}
