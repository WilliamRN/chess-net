using ChessNet.Data.Enums;
using ChessNet.Data.Models;
using System.Diagnostics;

namespace ChessNet.Data.Structs
{
    [DebuggerDisplay("{Destination}, isCapture: {IsCapture}")]
    public struct PieceMovement
    {
        private readonly bool _isPopulated;

        public BoardPosition Destination;
        public Piece PieceAtDestination;

        public PieceMovement(BoardPosition boardPosition, Piece piece = null)
        {
            Destination = boardPosition;
            PieceAtDestination = piece;
            _isPopulated = true;
        }

        public bool IsCapture => PieceAtDestination != null;
        public bool IsDefault => !_isPopulated;
        public bool IsValidPosition => _isPopulated;
    }
}