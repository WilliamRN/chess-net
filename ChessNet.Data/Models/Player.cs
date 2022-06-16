using ChessNet.Data.Enums;

namespace ChessNet.Data.Models
{
    public class Player
    {
        public Func<IEnumerable<Piece>> _piecesSelector { get; set; }

        public PieceColor Color { get; set; }
        public int Points { get; set; }

        public Player(PieceColor pieceColor, Func<IEnumerable<Piece>> piecesSelector)
        {
            Color = pieceColor;
            Points = 0;
            _piecesSelector = piecesSelector;
        }

        public IEnumerable<Piece> Pieces => _piecesSelector();
    }
}