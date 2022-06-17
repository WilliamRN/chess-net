using ChessNet.Data.Constants;
using ChessNet.Data.Enums;
using ChessNet.Data.Structs;

namespace ChessNet.Data.Models.Pieces
{
    public class Rook : Piece
    {
        public Rook(PieceColor pieceColor, BoardPosition boardPosition) 
            : base(pieceColor, boardPosition, PiecePoints.ROOK)
        {

        }

        public override IEnumerable<PieceMovement> GetMovements()
        {
            if (!IsInChessBoard) return default;

            // TODO: Castling
            // Castling may be done only if neither the king nor the rook has previously moved,
            // the squares between the king and the rook are unoccupied, the king is not in check,
            // and the king does not cross over or end up on a square attacked by an opposing piece.

            return CheckLineOfPositionsBasedOnPathStep(BoardDirectionSteps.HORIZONTAL_STEP)
                .Concat(CheckLineOfPositionsBasedOnPathStep(BoardDirectionSteps.VERTICAL_STEP));
        }
    }
}
