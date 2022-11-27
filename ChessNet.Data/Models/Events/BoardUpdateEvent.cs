using ChessNet.Data.Structs;

namespace ChessNet.Data.Models.Events
{
    public class BoardUpdateEvent
    {
        public BoardPosition Position { get; set; }
        public Piece PieceAtPosition { get; set; }

        public BoardUpdateEvent(BoardPosition position, Piece pieceAtPosition)
        {
            Position = position;
            PieceAtPosition = pieceAtPosition;
        }
    }
}
