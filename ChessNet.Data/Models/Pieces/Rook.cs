using ChessNet.Data.Constants;
using ChessNet.Data.Enums;
using ChessNet.Data.Structs;
using System.Diagnostics;

namespace ChessNet.Data.Models.Pieces
{
    [DebuggerDisplay("{Color} {this.GetType().Name} at {Position} inPlay:{IsInChessBoard}")]
    public class Rook : Piece
    {
        public Rook(PieceColor pieceColor, BoardPosition boardPosition) 
            : base(pieceColor, boardPosition, PiecePoints.ROOK)
        {

        }

        public override string GetSymbol()
        {
            return Color == PieceColor.White ? "♖" : "♜";
        }

        public override IEnumerable<PieceMovement> GetMovements()
        {
            if (!IsInChessBoard) return default;

            return CheckLineOfPositionsBasedOnPathStep(ChessBoard, Position, BoardDirectionSteps.HORIZONTAL_STEP, Color)
                .Concat(CheckLineOfPositionsBasedOnPathStep(ChessBoard, Position, BoardDirectionSteps.VERTICAL_STEP, Color));
        }

        internal static IEnumerable<Rook> GetRookAttackersFor(ChessBoard chessBoard, PieceColor color, BoardPosition position)
        {
            Piece attacker;
            var attackerColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;

            var piecePositions = CheckLineOfPositionsBasedOnPathStep(chessBoard, position, BoardDirectionSteps.HORIZONTAL_STEP, color)
                .Concat(CheckLineOfPositionsBasedOnPathStep(chessBoard, position, BoardDirectionSteps.VERTICAL_STEP, color))
                .Select(m => m.Destination);

            foreach (var piecePosition in piecePositions)
            {
                attacker = chessBoard.IsValidPosition(piecePosition) ? chessBoard.GetPiece(piecePosition) : null;

                if (attacker != null && attacker is Rook && attacker.Color == attackerColor)
                    yield return attacker as Rook;
            }
        }
    }
}
