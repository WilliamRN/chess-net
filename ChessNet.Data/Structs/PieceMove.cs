using ChessNet.Data.Enums;

namespace ChessNet.Data.Structs
{
    public struct PieceMovement
    {
        public BoardPosition Destination;
        public bool IsCapture;

        public PieceMovement(BoardPosition boardPosition, bool isCapture)
        {
            Destination = boardPosition;
            IsCapture = isCapture;
        }
    }
}