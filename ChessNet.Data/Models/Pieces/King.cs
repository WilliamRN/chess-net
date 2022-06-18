using ChessNet.Data.Constants;
using ChessNet.Data.Enums;
using ChessNet.Data.Structs;
using System.Diagnostics;

namespace ChessNet.Data.Models.Pieces
{
    [DebuggerDisplay("{Color} {this.GetType().Name} at {Position} inPlay:{IsInChessBoard}")]
    public class King : Piece
    {
        public King(PieceColor pieceColor, BoardPosition boardPosition)
            : base(pieceColor, boardPosition, PiecePoints.KING)
        {

        }

        public override string GetSymbol()
        {
            return Color == PieceColor.White ? "♔" : "♚";
        }

        public override IEnumerable<PieceMovement> GetMovements()
        {
            if (!IsInChessBoard) yield break;

            foreach (var validOffset in MoveOffsets.KING)
            {
                if (TryGetValidPieceMovement(ChessBoard, Position, validOffset, Color, out PieceMovement value))
                {
                    if (!ChessBoard.AttackersFor(this, value.Destination).Any())
                        yield return value;
                }
            }

            foreach (var m in Castling())
                yield return m;
        }

        private IEnumerable<PieceMovement> Castling()
        {
            // The king is not in check.
            if (ChessBoard.AttackersFor(this).Any())
                yield break;

            // The king has not moved.
            if (!IsFirstMove)
                yield break;

            // The rooks have not previously moved, and the rooks are on the same row.
            var rooks = ChessBoard
                .GetPieces(Color)
                .Where(p =>
                    p is Rook &&
                    p.IsFirstMove &&
                    p.Position.Row == Position.Row);

            foreach (var rook in rooks)
            {
                // Find the direction to the rook.
                int diff = Position.Column - rook.Position.Column;
                int step = diff > 0 ? -1 : 1;
                bool isPathOcuppied = false;

                // The squares between the king and the rook are unoccupied.
                for (int i = Position.Column
                    ; step > 0 ? i <= rook.Position.Column : i >= rook.Position.Column
                    ; i += step)
                {
                    if (i == Position.Column || i == rook.Position.Column)
                        continue;

                    if (ChessBoard.GetPiece(new BoardPosition(i, Position.Row)) != null)
                        isPathOcuppied = true;
                }

                if (!isPathOcuppied)
                {
                    BoardPosition rookCastlingPosition = new(Position.Column + step, Position.Row);
                    BoardPosition kingCastlingPosition = new(Position.Column + (step * 2), Position.Row);

                    // The king does not cross over or end up on a square attacked by an opposing piece.
                    if (!ChessBoard.AttackersFor(rook, rookCastlingPosition).Any() &&
                        !ChessBoard.AttackersFor(this, kingCastlingPosition).Any())
                    {
                        yield return new PieceMovement(kingCastlingPosition, rook, isCastling: true);
                    }
                }
            }
        }

        internal static IEnumerable<King> GetKingAttackersFor(ChessBoard chessBoard, PieceColor color, BoardPosition position)
        {
            Piece attacker;
            var attackerColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;
            BoardPosition piecePosition;

            foreach (var validOffset in MoveOffsets.KING)
            {
                piecePosition = position.GetOffset(validOffset);
                attacker = chessBoard.IsValidPosition(piecePosition) ? chessBoard.GetPiece(piecePosition) : null;

                if (attacker != null && attacker is King && attacker.Color == attackerColor)
                    yield return attacker as King;
            }
        }

        public bool IsCheck()
        {
            // A king that is under attack is said to be in check, and the player in check
            // must immediately remedy the situation. There are three possible ways to remove
            // the king from check:

            //      The king is moved to an adjacent non - threatened square.A king cannot castle
            //      to get out of check. A king can capture an adjacent enemy piece if that piece
            //      is not protected by another enemy piece.

            //      A piece is interposed between the king and the attacking piece to break the
            //      line of threat (not possible when the attacking piece is a knight or pawn, or
            //      when in double check).

            //      The attacking piece is captured(not possible when in double check, unless the
            //      king captures).

            // If none of the three options are available, the player's king has been checkmated,
            // and the player loses the game.

            // At amateur levels, when placing the opponent's king in check, it is common to
            // announce "check", but this is not required by the rules of chess.

            throw new NotImplementedException();
        }
    }
}
