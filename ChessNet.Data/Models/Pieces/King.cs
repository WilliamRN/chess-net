using ChessNet.Data.Constants;
using ChessNet.Data.Enums;
using ChessNet.Data.Structs;

namespace ChessNet.Data.Models.Pieces
{
    public class King : Piece
    {
        public King(PieceColor pieceColor, BoardPosition boardPosition)
            : base(pieceColor, boardPosition, PiecePoints.KING)
        {

        }

        public override IEnumerable<PieceMovement> GetMovements()
        {
            if (!IsInChessBoard) yield break;

            foreach (var validOffset in MoveOffsets.KING)
            {
                if (TryGetValidPieceMovement(validOffset, out PieceMovement value))
                    yield return value;
            }

            foreach (var m in Castling())
                yield return m;
        }

        private IEnumerable<PieceMovement> Castling()
        {
            // The king has not moved.
            if (!IsFirstMove)
                yield break;

            // The king is not in check.
            if (ChessBoard.IsPositionUnderAttackFor(this))
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
                    if (!ChessBoard.IsPositionUnderAttackFor(Color, rookCastlingPosition) &&
                        !ChessBoard.IsPositionUnderAttackFor(Color, kingCastlingPosition))
                    {
                        yield return new PieceMovement(kingCastlingPosition, rook, isCastling: true);
                    }
                }
            }
        }
    }
}
