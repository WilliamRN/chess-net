using ChessNet.Data.Enums;
using ChessNet.Data.Structs;

namespace ChessNet.Data.Models.Pieces
{
    public class Knight : Piece
    {
        public Knight(PieceColor pieceColor, BoardPosition boardPosition) 
            : base(pieceColor, PieceType.Knight, boardPosition)
        {

        }

        public override IEnumerable<PieceMovement> GetMovements(ChessBoard chessBoard)
        {
            throw new NotImplementedException();
        }
    }
}
