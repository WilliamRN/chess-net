using ChessNet.Data.Enums;
using ChessNet.Data.Models;
using ChessNet.Data.Structs;

namespace ChessNet.Data.Rules
{
    public static class PieceMovements
    {
        public static IEnumerable<PieceMovement> GetMoves(this Piece piece, ChessBoard chessBoard)
        {
            return piece?.Type switch
            {
                PieceType.Pawn => piece.GetPawnMovements(chessBoard),
                PieceType.Bishop => throw new NotImplementedException(),
                PieceType.Knight => throw new NotImplementedException(),
                PieceType.Rook => throw new NotImplementedException(),
                PieceType.Queen => throw new NotImplementedException(),
                PieceType.King => throw new NotImplementedException(),
                _ => throw new InvalidOperationException(),
            };
        }
    }
}
