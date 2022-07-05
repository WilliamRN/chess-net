using ChessNet.Data.Enums;
using ChessNet.Data.Models;
using System.Diagnostics;

namespace ChessNet.Data.Structs
{
    [DebuggerDisplay("{Destination}, isDefault: {IsDefault}, isOcuppied: {PieceAtDestination != null}")]
    public struct Movement
    {
        private readonly bool _isPopulated;

        public BoardPosition Destination;
        public Piece PieceAtDestination;
        public bool IsEnPassant { get; private set; }
        public bool IsCastling { get; private set; }

        public bool IsDefault => !_isPopulated;
        public bool IsValidPosition => _isPopulated;

        public Movement(BoardPosition boardPosition, Piece pieceAtDestination = null, bool isEnPassant = false, bool isCastling = false)
        {
            Destination = boardPosition;
            PieceAtDestination = pieceAtDestination;
            IsEnPassant = isEnPassant;
            IsCastling = isCastling;
            _isPopulated = true;
        }

        public bool IsCaptureFor(PieceColor color) =>
            PieceAtDestination != null && PieceAtDestination.Color != color;
    }
}