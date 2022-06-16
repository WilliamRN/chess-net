using ChessNet.Data.Enums;
using ChessNet.Data.Models;
using ChessNet.Data.Structs;

namespace ChessNet.Data.Rules
{
    public static class PawnMovements
    {
        public static IEnumerable<PieceMovement> GetPawnMovements(this Piece piece, ChessBoard chessBoard)
        {
            PieceType expectedPiece = PieceType.Pawn;
            BoardPosition position;
            bool isOcuppied;

            if (piece == null)
                throw new ArgumentNullException(nameof(piece), $"expected {expectedPiece}");

            if (piece.Type != Enums.PieceType.Pawn)
                throw new ArgumentException($"invalid piece, expected {expectedPiece} but got {piece.Type}");

            // Can move but not capture ahead
            if (piece.IsFirstMove)
            {
                position = piece.Position.GetOffset(0, (piece.IsWhite ? 2 : -2));
                isOcuppied = chessBoard.GetPiece(position) != null;
                if (!isOcuppied && chessBoard.IsValidPosition(position)) yield return new PieceMovement(position, false);
            }

            position = piece.Position.GetOffset(0, (piece.IsWhite ? 1 : -1));
            isOcuppied = chessBoard.GetPiece(position) != null;
            if (!isOcuppied && chessBoard.IsValidPosition(position)) yield return new PieceMovement(position, false);

            // Can only capture on imediate diagonals
            position = piece.Position.GetOffset(1, (piece.IsWhite ? 1 : -1));
            isOcuppied = chessBoard.GetPiece(position) != null;
            if (isOcuppied && chessBoard.IsValidPosition(position)) yield return new PieceMovement(position, true);

            position = piece.Position.GetOffset(-1, (piece.IsWhite ? 1 : -1));
            isOcuppied = chessBoard.GetPiece(position) != null;
            if (isOcuppied && chessBoard.IsValidPosition(position)) yield return new PieceMovement(position, true);
        }
    }
}
