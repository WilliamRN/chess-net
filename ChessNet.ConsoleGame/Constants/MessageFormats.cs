﻿namespace ChessNet.ConsoleGame.Constants
{
    internal static class MessageFormats
    {
        internal const string GAME_STARTED = "Game board is set! It's {0}'s turn.";
        internal const string SURRENDERED = "{0} has surrendered!";
        internal const string MOVED_PIECE = "Moved {0} to {1}.{2}";
        internal const string MOVED_PIECE_THEN_CAPTURE = " {0} has captured a {1}!";
        internal const string NEW_TURN = "It's {0}'s turn.";
        internal const string COULD_NOT_MOVE = "Could not move from {0} to {1}: {2}";
    }
}
