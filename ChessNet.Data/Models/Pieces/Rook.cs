using ChessNet.Data.Enums;
using ChessNet.Data.Structs;

namespace ChessNet.Data.Models.Pieces
{
    public class Rook : Piece
    {
        public Rook(PieceColor pieceColor, BoardPosition boardPosition) 
            : base(pieceColor, PieceType.Rook, boardPosition)
        {

        }

        public override IEnumerable<PieceMovement> GetMovements(ChessBoard chessBoard)
        {
            throw new NotImplementedException();
        }
    }
}
