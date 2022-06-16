using ChessNet.Data.Models;
using ChessNet.Data.Structs;

namespace ChessNet.Data.Extensions
{
    public static class GameExtensions
    {
        public static char AsLetter(this int column)
        {
            char baseA = 'A';
            return (char)(baseA + column);
        }

        public static bool TryMoveTo(this IEnumerable<PieceMovement> pieceMovements, int column, int row, out PieceMovement pieceMovement)
        {
            pieceMovement = default;
            bool result = false;

            if (pieceMovements == null || !pieceMovements.Any()) return result;

            if (pieceMovements.Any(m => m.Destination.Column == column && m.Destination.Row == row))
            {
                pieceMovement = pieceMovements
                    .First(m => m.Destination.Column == column && m.Destination.Row == row);

                result = true;
            }

            return result;
        }

        public static bool TryMoveTo(this IEnumerable<PieceMovement> pieceMovements, BoardPosition boardPosition, out PieceMovement pieceMovement) =>
            pieceMovements.TryMoveTo(boardPosition.Column, boardPosition.Row, out pieceMovement);

        public static bool TryMoveTo(this IEnumerable<PieceMovement> pieceMovements, Piece piece, out PieceMovement pieceMovement) =>
            pieceMovements.TryMoveTo(piece.Position.Column, piece.Position.Row, out pieceMovement);
    }
}
