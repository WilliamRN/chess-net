using ChessNet.Data.Constants;
using ChessNet.Data.Enums;
using ChessNet.Data.Structs;

namespace ChessNet.Data.Models
{
    public abstract class Piece
    {
        public PieceColor Color { get; private set; }
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

        public Piece(PieceColor pieceColor, BoardPosition boardPosition, int points = PiecePoints.DEFAULT)
        {
            Color = pieceColor;
            Position = boardPosition;
            IsFirstMove = true;
            Points = points;
        }

        public abstract IEnumerable<PieceMovement> GetMovements(ChessBoard chessBoard);
    }
}