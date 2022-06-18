using ChessNet.Data.Enums;
using ChessNet.Data.Models;
using ChessNet.Data.Models.Pieces;
using ChessNet.Data.Structs;

namespace ChessNet.Data.Constants
{
    public static class InitialBoardPlacements
    {
        public static readonly IEnumerable<Piece> DEFAULT = new List<Piece>
        {
            // White
            new Rook(PieceColor.White, new BoardPosition(0, 0)),
            new Knight(PieceColor.White, new BoardPosition(1, 0)),
            new Bishop(PieceColor.White, new BoardPosition(2, 0)),
            new Queen(PieceColor.White, new BoardPosition(3, 0)),
            new King(PieceColor.White, new BoardPosition(4, 0)),
            new Bishop(PieceColor.White, new BoardPosition(5, 0)),
            new Knight(PieceColor.White, new BoardPosition(6, 0)),
            new Rook(PieceColor.White, new BoardPosition(7, 0)),

            new Pawn(PieceColor.White, new BoardPosition(0, 1)),
            new Pawn(PieceColor.White, new BoardPosition(1, 1)),
            new Pawn(PieceColor.White, new BoardPosition(2, 1)),
            new Pawn(PieceColor.White, new BoardPosition(3, 1)),
            new Pawn(PieceColor.White, new BoardPosition(4, 1)),
            new Pawn(PieceColor.White, new BoardPosition(5, 1)),
            new Pawn(PieceColor.White, new BoardPosition(6, 1)),
            new Pawn(PieceColor.White, new BoardPosition(7, 1)),

            // Black
            new Pawn(PieceColor.Black, new BoardPosition(0, 6)),
            new Pawn(PieceColor.Black, new BoardPosition(1, 6)),
            new Pawn(PieceColor.Black, new BoardPosition(2, 6)),
            new Pawn(PieceColor.Black, new BoardPosition(3, 6)),
            new Pawn(PieceColor.Black, new BoardPosition(4, 6)),
            new Pawn(PieceColor.Black, new BoardPosition(5, 6)),
            new Pawn(PieceColor.Black, new BoardPosition(6, 6)),
            new Pawn(PieceColor.Black, new BoardPosition(7, 6)),

            new Rook(PieceColor.Black, new BoardPosition(0, 7)),
            new Knight(PieceColor.Black, new BoardPosition(1, 7)),
            new Bishop(PieceColor.Black, new BoardPosition(2, 7)),
            new Queen(PieceColor.Black, new BoardPosition(3, 7)),
            new King(PieceColor.Black, new BoardPosition(4, 7)),
            new Bishop(PieceColor.Black, new BoardPosition(5, 7)),
            new Knight(PieceColor.Black, new BoardPosition(6, 7)),
            new Rook(PieceColor.Black, new BoardPosition(7, 7)),
        };
    }
}
