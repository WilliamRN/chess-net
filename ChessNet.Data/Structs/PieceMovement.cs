using ChessNet.Data.Enums;
using ChessNet.Data.Models;
using System.Diagnostics;

namespace ChessNet.Data.Structs
{
    [DebuggerDisplay("{FromPosition.AsString()} {(FromPiece != null ? FromPiece.GetType().Name : string.Empty)} to {ToPosition.AsString()} {(ToPiece != null ? ToPiece.GetType().Name : string.Empty)}")]
    public struct PieceMovement
    {
        private readonly bool _isPopulated;

        public bool IsSurrender { get; private set; }

        public Piece FromPiece { get; private set; }
        public BoardPosition FromPosition { get; private set; }

        public Piece ToPiece { get; private set; }
        public BoardPosition ToPosition { get; private set; }

        public bool IsDefault => !_isPopulated;

        public static PieceMovement Forfeit =>
            new(new BoardPosition(), new BoardPosition())
            {
                IsSurrender = true,
            };

        public PieceMovement(BoardPosition fromPosition, BoardPosition toPosition)
        {
            _isPopulated = true;
            FromPiece = null;
            FromPosition = fromPosition;
            ToPiece = null;
            ToPosition = toPosition;
            IsSurrender = false;
        }

        public PieceMovement(Piece fromPiece, Piece toPiece)
        {
            _isPopulated = true;
            FromPiece = fromPiece;
            FromPosition = fromPiece.Position;
            ToPiece = toPiece;
            ToPosition = toPiece.Position;
            IsSurrender = false;
        }

        public PieceMovement(Piece fromPiece, BoardPosition toPosition, Piece toPiece = null)
        {
            _isPopulated = true;
            FromPiece = fromPiece;
            FromPosition = fromPiece.Position;
            ToPiece = toPiece;
            ToPosition = toPosition;
            IsSurrender = false;
        }

        public PieceMovement(Piece fromPiece, BoardPosition fromPosition, BoardPosition toPosition, Piece toPiece = null)
        {
            _isPopulated = true;
            FromPiece = fromPiece;
            FromPosition = fromPosition;
            ToPiece = toPiece;
            ToPosition = toPosition;
            IsSurrender = false;
        }
    }
}