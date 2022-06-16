using ChessNet.Data.Enums;
using System.Diagnostics;

namespace ChessNet.Data.Structs
{
    [DebuggerDisplay("{Destination}, isCapture: {IsCapture}")]
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