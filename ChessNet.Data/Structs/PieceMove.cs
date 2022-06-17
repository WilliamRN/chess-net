using ChessNet.Data.Enums;
using ChessNet.Data.Models;
using System.Diagnostics;

namespace ChessNet.Data.Structs
{
    [DebuggerDisplay("{Destination}, isDefault: {IsDefault}, isOcuppied: {PieceAtDestination != null}")]
    public struct PieceMovement
    {
        private readonly bool _isPopulated;

        public BoardPosition Destination;
        public Piece PieceAtDestination;
        public bool IsEnPassant { get; private set; }

        public PieceMovement(BoardPosition boardPosition, Piece piece = null, bool isEnPassant = false)
        {
            Destination = boardPosition;
            PieceAtDestination = piece;
            IsEnPassant = isEnPassant;
            _isPopulated = true;
        }

        public bool IsCaptureFor(PieceColor color)
        {
            return PieceAtDestination != null && PieceAtDestination.Color != color;
        }

        public bool IsDefault => !_isPopulated;
        public bool IsValidPosition => _isPopulated;
    }
}