using ChessNet.Data.Enums;
using ChessNet.Data.Models;
using ChessNet.Data.Structs;

namespace ChessNet.Data.Constants
{
    public static class InitialStates
    {
        public static IEnumerable<Piece> DEFAULT = new List<Piece>
        {
            // White
            new Piece(PieceColor.White, PieceType.Rook, new BoardPosition(0, 0)),
            new Piece(PieceColor.White, PieceType.Knight, new BoardPosition(1, 0)),
            new Piece(PieceColor.White, PieceType.Bishop, new BoardPosition(2, 0)),
            new Piece(PieceColor.White, PieceType.Queen, new BoardPosition(3, 0)),
            new Piece(PieceColor.White, PieceType.King, new BoardPosition(4, 0)),
            new Piece(PieceColor.White, PieceType.Bishop, new BoardPosition(5, 0)),
            new Piece(PieceColor.White, PieceType.Knight, new BoardPosition(6, 0)),
            new Piece(PieceColor.White, PieceType.Rook, new BoardPosition(7, 0)),

            new Piece(PieceColor.White, PieceType.Pawn, new BoardPosition(0, 1)),
            new Piece(PieceColor.White, PieceType.Pawn, new BoardPosition(1, 1)),
            new Piece(PieceColor.White, PieceType.Pawn, new BoardPosition(2, 1)),
            new Piece(PieceColor.White, PieceType.Pawn, new BoardPosition(3, 1)),
            new Piece(PieceColor.White, PieceType.Pawn, new BoardPosition(4, 1)),
            new Piece(PieceColor.White, PieceType.Pawn, new BoardPosition(5, 1)),
            new Piece(PieceColor.White, PieceType.Pawn, new BoardPosition(6, 1)),
            new Piece(PieceColor.White, PieceType.Pawn, new BoardPosition(7, 1)),

            // Black
            new Piece(PieceColor.Black, PieceType.Pawn, new BoardPosition(0, 6)),
            new Piece(PieceColor.Black, PieceType.Pawn, new BoardPosition(1, 6)),
            new Piece(PieceColor.Black, PieceType.Pawn, new BoardPosition(2, 6)),
            new Piece(PieceColor.Black, PieceType.Pawn, new BoardPosition(3, 6)),
            new Piece(PieceColor.Black, PieceType.Pawn, new BoardPosition(4, 6)),
            new Piece(PieceColor.Black, PieceType.Pawn, new BoardPosition(5, 6)),
            new Piece(PieceColor.Black, PieceType.Pawn, new BoardPosition(6, 6)),
            new Piece(PieceColor.Black, PieceType.Pawn, new BoardPosition(7, 6)),

            new Piece(PieceColor.Black, PieceType.Rook, new BoardPosition(0, 7)),
            new Piece(PieceColor.Black, PieceType.Knight, new BoardPosition(1, 7)),
            new Piece(PieceColor.Black, PieceType.Bishop, new BoardPosition(2, 7)),
            new Piece(PieceColor.Black, PieceType.Queen, new BoardPosition(3, 7)),
            new Piece(PieceColor.Black, PieceType.King, new BoardPosition(4, 7)),
            new Piece(PieceColor.Black, PieceType.Bishop, new BoardPosition(5, 7)),
            new Piece(PieceColor.Black, PieceType.Knight, new BoardPosition(6, 7)),
            new Piece(PieceColor.Black, PieceType.Rook, new BoardPosition(7, 7)),
        };
    }
}
