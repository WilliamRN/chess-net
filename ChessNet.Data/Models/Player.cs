using ChessNet.Data.Enums;

namespace ChessNet.Data.Models
{
    public class Player
    {
        public PieceColor Color { get; set; }
        public IEnumerable<Piece> Pieces { get; set; }
        public int Points { get; set; }

        public Player(PieceColor pieceColor)
        {
            Color = pieceColor;
            Pieces = new List<Piece>();
            Points = 0;
        }

        public Player(PieceColor pieceColor, IEnumerable<Piece> pieces) : this(pieceColor)
        {
            Pieces = pieces;
        }
    }
}