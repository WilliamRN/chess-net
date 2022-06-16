using ChessNet.Data.Enums;
using ChessNet.Data.Structs;

namespace ChessNet.Data.Models.Pieces
{
    public class Bishop : Piece
    {
        public Bishop(PieceColor pieceColor, BoardPosition boardPosition) 
            : base(pieceColor, PieceType.Bishop, boardPosition)
        {

        }

        public override IEnumerable<PieceMovement> GetMovements(ChessBoard chessBoard)
        {
            throw new NotImplementedException();
        }
    }
}
