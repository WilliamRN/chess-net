using ChessNet.Data.Enums;
using ChessNet.Data.Structs;

namespace ChessNet.Data.Models
{
    public abstract class Piece
    {
        public PieceColor Color { get; private set; }
        public PieceType Type { get; private set; }
        public bool IsFirstMove { get; private set; }
        public int Points { get; private set; }

        private BoardPosition _position { get; set; }
        public BoardPosition Position
        {
            get => _position;
            set
            {
                IsFirstMove = false;
                _position = value;
            }
        }

        public bool IsWhite => Color == PieceColor.White;

        public Piece(PieceColor pieceColor, PieceType pieceType, BoardPosition boardPosition)
        {
            Color = pieceColor;
            Type = pieceType;
            Position = boardPosition;
            IsFirstMove = true;
            Points = 1; // TODO: make points relative to piece type.
        }

        public abstract IEnumerable<PieceMovement> GetMovements(ChessBoard chessBoard);
    }
}