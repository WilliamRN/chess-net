using ChessNet.Data.Constants;
using ChessNet.Data.Enums;
using ChessNet.Data.Structs;
using System.Diagnostics;

namespace ChessNet.Data.Models.Pieces
{
    [DebuggerDisplay("{Color} {this.GetType().Name} at {Position} inPlay:{IsInChessBoard}")]
    public class Knight : Piece
    {
        public Knight(PieceColor pieceColor, BoardPosition boardPosition) 
            : base(pieceColor, boardPosition, PiecePoints.KNIGHT)
        {

        }

        public override string GetSymbol()
        {
            return Color == PieceColor.White ? "♘" : "♞";
        }

        public override IEnumerable<PieceMovement> GetMovements()
        {
            if (!IsInChessBoard) yield break;

            foreach (var validOffset in MoveOffsets.KNIGHT)
            {
                if (TryGetValidPieceMovement(Board, Position, validOffset, Color, out PieceMovement value))
                    if (IsValidMoveForCurrentKingPosition(this, value))
                        yield return value;
            }
        }

        internal static IEnumerable<Knight> GetKnightAttackersFor(ChessBoard chessBoard, PieceColor color, BoardPosition position)
        {
            Piece attacker;
            var attackerColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;
            BoardPosition piecePosition;

            foreach (var validOffset in MoveOffsets.KNIGHT)
            {
                piecePosition = position.GetOffset(validOffset);
                attacker = chessBoard.IsValidPosition(piecePosition) ? chessBoard.GetPiece(piecePosition) : null;

                if (attacker != null && attacker is Knight && attacker.Color == attackerColor)
                    yield return attacker as Knight;
            }
        }
    }
}
