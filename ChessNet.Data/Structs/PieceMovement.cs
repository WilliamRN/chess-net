using ChessNet.Data.Enums;
using ChessNet.Data.Models;
using System.Diagnostics;

namespace ChessNet.Data.Structs
{
    [DebuggerDisplay("{FromPosition.AsString()} {(FromPiece != null ? FromPiece.GetType().Name : string.Empty)} to {ToPosition.AsString()} {(ToPiece != null ? ToPiece.GetType().Name : string.Empty)}")]
    public struct PieceMovement
    {
        public Piece FromPiece { get; private set; }
        public BoardPosition FromPosition { get; private set; }
        
        public Piece ToPiece { get; private set; }
        public BoardPosition ToPosition { get; private set; }

        private readonly bool _isPopulated;
        public bool IsDefault => !_isPopulated;

        public PieceMovement(BoardPosition fromPosition, BoardPosition toPosition)
        {
            FromPiece = null;
            FromPosition = fromPosition;
            ToPiece = null;
            ToPosition = toPosition;
            _isPopulated = true;
        }

        public PieceMovement(Piece fromPiece, Piece toPiece)
        {
            FromPiece = fromPiece;
            FromPosition = fromPiece.Position;
            ToPiece = toPiece;
            ToPosition = toPiece.Position;
            _isPopulated = true;
        }

        public PieceMovement(Piece fromPiece, BoardPosition toPosition, Piece toPiece = null)
        {
            FromPiece = fromPiece;
            FromPosition = fromPiece.Position;
            ToPiece = toPiece;
            ToPosition = toPosition;
            _isPopulated = true;
        }

        public PieceMovement(Piece fromPiece, BoardPosition fromPosition, BoardPosition toPosition, Piece toPiece = null)
        {
            FromPiece = fromPiece;
            FromPosition = fromPosition;
            ToPiece = toPiece;
            ToPosition = toPosition;
            _isPopulated = true;
        }
    }
}