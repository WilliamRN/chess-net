using ChessNet.Data.Models;
using ChessNet.Data.Structs;

namespace ChessNet.Data.Extensions
{
    internal static class GameExtensions
    {
        public static string AsLetter(this int column)
        {
            return column switch
            {
                1 => "A",
                2 => "B",
                3 => "C",
                4 => "D",
                5 => "E",
                6 => "F",
                7 => "G",
                8 => "H",
                _ => "",
            };
        }

        public static bool TryGetAt(this IEnumerable<PieceMovement> pieceMovements, int column, int row, out PieceMovement pieceMovement)
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

        public static bool TryGetAt(this IEnumerable<PieceMovement> pieceMovements, BoardPosition boardPosition, out PieceMovement pieceMovement) =>
            pieceMovements.TryGetAt(boardPosition.Column, boardPosition.Row, out pieceMovement);

        public static bool TryGetAt(this IEnumerable<PieceMovement> pieceMovements, Piece piece, out PieceMovement pieceMovement) =>
            pieceMovements.TryGetAt(piece.Position.Column, piece.Position.Row, out pieceMovement);
    }
}
