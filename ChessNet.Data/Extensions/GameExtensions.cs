using ChessNet.Data.Models;
using ChessNet.Data.Structs;

namespace ChessNet.Data.Extensions
{
    public static class GameExtensions
    {
        public static string ToColumnAnnotation(this int column)
        {
            string result = "";

            char baseA = 'A';
            char maxZ = 'Z';

            int baseChar = maxZ - baseA + 1; // 26 possible values.
            int division = column + 1;
            int remainder;

            do
            {
                remainder = division % baseChar;

                if (remainder == 0)
                {
                    result += maxZ;
                    division -= baseChar;
                }
                else
                {
                    result += (char)(baseA + remainder - 1);
                }

                division = (int)Math.Floor((double)division / baseChar);

            } while (division > 0);

            var resultAsArray = result.ToCharArray();
            Array.Reverse(resultAsArray);

            return new string(resultAsArray);
        }

        public static int ToColumnInteger(this string column)
        {
            var reversed = column.ToUpper().ToCharArray();
            Array.Reverse(reversed);

            char baseA = 'A';
            char maxZ = 'Z';

            int baseChar = maxZ - baseA + 1; // 26 possible values.
            int result = 0;
            int multiplier = 1;
            int multiplying;

            foreach (char c in reversed)
            {
                multiplying = c - baseA + 1;
                result += multiplying * multiplier;
                multiplier *= baseChar;
            }

            return result - 1;
        }

        public static bool TryMoveTo(this IEnumerable<Movement> pieceMovements, int column, int row, out Movement pieceMovement)
        {
            pieceMovement = default;
            bool result = false;

            var list = pieceMovements.ToList();

            if (list == null || !list.Any()) return result;

            pieceMovement = list.FirstOrDefault(m => m.Destination.Column == column && m.Destination.Row == row);
            result = !pieceMovement.IsDefault;

            return result;
        }

        public static bool TryMoveTo(this IEnumerable<Movement> pieceMovements, BoardPosition boardPosition, out Movement pieceMovement) =>
            pieceMovements.TryMoveTo(boardPosition.Column, boardPosition.Row, out pieceMovement);

        public static bool TryMoveTo(this IEnumerable<Movement> pieceMovements, Piece piece, out Movement pieceMovement) =>
            pieceMovements.TryMoveTo(piece.Position.Column, piece.Position.Row, out pieceMovement);
    }
}
