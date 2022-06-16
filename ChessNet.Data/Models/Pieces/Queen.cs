using ChessNet.Data.Enums;
using ChessNet.Data.Structs;

namespace ChessNet.Data.Models.Pieces
{
    public class Queen : Piece
    {
        public Queen(PieceColor pieceColor, BoardPosition boardPosition) 
            : base(pieceColor, PieceType.Queen, boardPosition)
        {

        }

        public override IEnumerable<PieceMovement> GetMovements(ChessBoard chessBoard)
        {
            throw new NotImplementedException();
        }
    }
}
