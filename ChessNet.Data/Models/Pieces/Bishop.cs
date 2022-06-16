using ChessNet.Data.Constants;
using ChessNet.Data.Enums;
using ChessNet.Data.Structs;

namespace ChessNet.Data.Models.Pieces
{
    public class Bishop : Piece
    {
        public Bishop(PieceColor pieceColor, BoardPosition boardPosition) 
            : base(pieceColor, boardPosition, PiecePoints.BISHOP)
        {

        }

        public override IEnumerable<PieceMovement> GetMovements(ChessBoard chessBoard)
        {
            return CheckLineOfPositionsBasedOnPathStep(BoardDirectionSteps.DIAGONAL_STEP_FOWARD, chessBoard)
                .Concat(CheckLineOfPositionsBasedOnPathStep(BoardDirectionSteps.DIAGONAL_STEP_BACK, chessBoard));
        }
    }
}
