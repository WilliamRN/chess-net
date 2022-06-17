using ChessNet.Data.Constants;
using ChessNet.Data.Enums;
using ChessNet.Data.Structs;

namespace ChessNet.Data.Models.Pieces
{
    public class Queen : Piece
    {
        public Queen(PieceColor pieceColor, BoardPosition boardPosition)
            : base(pieceColor, boardPosition, PiecePoints.QUEEN)
        {

        }

        public override IEnumerable<PieceMovement> GetMovements()
        {
            if (!IsInChessBoard) return default;

            return CheckLineOfPositionsBasedOnPathStep(BoardDirectionSteps.HORIZONTAL_STEP)
                .Concat(CheckLineOfPositionsBasedOnPathStep(BoardDirectionSteps.VERTICAL_STEP))
                .Concat(CheckLineOfPositionsBasedOnPathStep(BoardDirectionSteps.DIAGONAL_STEP_BACK))
                .Concat(CheckLineOfPositionsBasedOnPathStep(BoardDirectionSteps.DIAGONAL_STEP_FOWARD));
        }
    }
}
