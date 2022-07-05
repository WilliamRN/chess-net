using ChessNet.Data.Structs;

namespace ChessNet.Data.Constants
{
    public static class MoveOffsets
    {
        internal static readonly IEnumerable<BoardPosition> KING = new List<BoardPosition>()
        {
            new BoardPosition(-1, -1),
            new BoardPosition(-1, 0),
            new BoardPosition(-1, 1),

            new BoardPosition(0, 1),
            new BoardPosition(0, -1),

            new BoardPosition(1, 1),
            new BoardPosition(1, 0),
            new BoardPosition(1, -1),
        };

        internal static readonly IEnumerable<BoardPosition> KNIGHT = new List<BoardPosition>()
        {
            new BoardPosition(1, 2),
            new BoardPosition(2, 1),

            new BoardPosition(2, -1),
            new BoardPosition(1, -2),

            new BoardPosition(-1, -2),
            new BoardPosition(-2, -1),

            new BoardPosition(-2, 1),
            new BoardPosition(-1, 2),
        };
    }
}