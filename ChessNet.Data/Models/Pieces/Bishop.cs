using ChessNet.Data.Constants;
using ChessNet.Data.Enums;
using ChessNet.Data.Structs;
using System.Diagnostics;

namespace ChessNet.Data.Models.Pieces
{
    [DebuggerDisplay("{Color} {this.GetType().Name} at {Position} inPlay:{IsInChessBoard}")]
    public class Bishop : Piece
    {
        public Bishop(PieceColor pieceColor, BoardPosition boardPosition) 
            : base(pieceColor, boardPosition, PiecePoints.BISHOP)
        {

        }

        public override string GetSymbol()
        {
            return Color == PieceColor.White ? "♗" : "♝";
        }

        public override IEnumerable<PieceMovement> GetMovements()
        {
            if (!IsInChessBoard) return default;

            return CheckLineOfPositionsBasedOnPathStep(ChessBoard, Position, BoardDirectionSteps.DIAGONAL_STEP_FOWARD, Color)
                .Concat(CheckLineOfPositionsBasedOnPathStep(ChessBoard, Position, BoardDirectionSteps.DIAGONAL_STEP_BACK, Color));
        }

        internal static IEnumerable<Bishop> GetBishopAttackersFor(ChessBoard chessBoard, PieceColor color, BoardPosition position)
        {
            Piece attacker;
            var attackerColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;

            var piecePositions = CheckLineOfPositionsBasedOnPathStep(chessBoard, position, BoardDirectionSteps.DIAGONAL_STEP_FOWARD, color)
                .Concat(CheckLineOfPositionsBasedOnPathStep(chessBoard, position, BoardDirectionSteps.DIAGONAL_STEP_BACK, color))
                .Select(m => m.Destination);

            foreach (var piecePosition in piecePositions)
            {
                attacker = chessBoard.IsValidPosition(piecePosition) ? chessBoard.GetPiece(piecePosition) : null;

                if (attacker != null && attacker is Bishop && attacker.Color == attackerColor)
                    yield return attacker as Bishop;
            }
        }
    }
}
