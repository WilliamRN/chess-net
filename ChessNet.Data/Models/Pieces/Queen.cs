using ChessNet.Data.Constants;
using ChessNet.Data.Enums;
using ChessNet.Data.Structs;
using System.Diagnostics;

namespace ChessNet.Data.Models.Pieces
{
    [DebuggerDisplay("{Color} {this.GetType().Name} at {Position} inPlay:{IsInChessBoard}")]
    public class Queen : Piece
    {
        public Queen(PieceColor pieceColor, BoardPosition boardPosition)
            : base(pieceColor, boardPosition, PiecePoints.QUEEN)
        {

        }

        public override string Symbol => Color == PieceColor.White ? "♕" : "♛";

        public override PieceType PieceType => PieceType.Queen;

        public override IEnumerable<Movement> GetMovements()
        {
            if (!IsInChessBoard) return default;

            return CheckLineOfPositionsBasedOnPathStep(Board, Position, BoardDirectionSteps.HORIZONTAL_STEP, Color)
                .Concat(CheckLineOfPositionsBasedOnPathStep(Board, Position, BoardDirectionSteps.VERTICAL_STEP, Color))
                .Concat(CheckLineOfPositionsBasedOnPathStep(Board, Position, BoardDirectionSteps.DIAGONAL_STEP_BACK, Color))
                .Concat(CheckLineOfPositionsBasedOnPathStep(Board, Position, BoardDirectionSteps.DIAGONAL_STEP_FOWARD, Color))
                .Where(m => IsValidMoveForCurrentKingPosition(this, m));
        }

        public override IEnumerable<Piece> GetAttackersFor(ChessBoard chessBoard, PieceColor color, BoardPosition position)
        {
            Piece attacker;
            var attackerColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;

            var piecePositions = CheckLineOfPositionsBasedOnPathStep(chessBoard, position, BoardDirectionSteps.HORIZONTAL_STEP, color)
                .Concat(CheckLineOfPositionsBasedOnPathStep(chessBoard, position, BoardDirectionSteps.VERTICAL_STEP, color))
                .Concat(CheckLineOfPositionsBasedOnPathStep(chessBoard, position, BoardDirectionSteps.DIAGONAL_STEP_BACK, color))
                .Concat(CheckLineOfPositionsBasedOnPathStep(chessBoard, position, BoardDirectionSteps.DIAGONAL_STEP_FOWARD, color))
                .Where(m => m.IsCaptureFor(color))
                .Select(m => m.Destination);

            foreach (var piecePosition in piecePositions)
            {
                attacker = chessBoard.IsValidPosition(piecePosition) ? chessBoard.GetPiece(piecePosition) : null;

                if (attacker != null && attacker is Queen && attacker.Color == attackerColor)
                    yield return attacker as Queen;
            }
        }
    }
}
