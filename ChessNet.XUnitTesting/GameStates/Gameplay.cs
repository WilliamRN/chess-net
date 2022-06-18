using ChessNet.Data.Enums;
using ChessNet.Data.Models;
using ChessNet.Data.Models.Pieces;
using ChessNet.Data.Structs;

namespace ChessNet.XUnitTesting.GameExecution
{
    public class Gameplay
    {
        [Fact]
        public void When_KingIsCaptured_Then_GameEnds()
        {
            List<Piece> pieces = new()
            {
                new King(PieceColor.White, new BoardPosition("E3")),
                new King(PieceColor.Black, new BoardPosition("E4")),
            };

            ChessGame game = new(pieces);

            var wKing = game.Board.GetPiece(new BoardPosition("E3")) as King;
            var bKing = game.Board.GetPiece(new BoardPosition("E4")) as King;

            game.MovePiece(wKing, new BoardPosition("E4"));

            Assert.Equal(GameStates.End, game.State);
        }

        [Fact]
        public void When_KingIsUnderAttackAndNoMovesAreAvailable_Then_StateIsCheckMate()
        {
            List<Piece> pieces = new()
            {
                new Rook(PieceColor.Black, new BoardPosition("F8")),
                new Queen(PieceColor.Black, new BoardPosition("H7")),
                new Knight(PieceColor.White, new BoardPosition("G6")),
                new Pawn(PieceColor.White, new BoardPosition("B4")),
                new King(PieceColor.Black, new BoardPosition("D4")),
                new Queen(PieceColor.White, new BoardPosition("G3")),
                new Bishop(PieceColor.White, new BoardPosition("A2")),
                new Rook(PieceColor.White, new BoardPosition("D1")),
                new King(PieceColor.White, new BoardPosition("G1")),
            };

            ChessGame game = new(pieces);

            var bKing = game.Board.GetPiece(new BoardPosition("D4"));
            var wQueen = game.Board.GetPiece(new BoardPosition("G3"));

            var blackKingMovesPreviously = bKing.GetMovements().ToList();

            game.MovePiece(wQueen, new BoardPosition("F3"));

            Assert.Equal(GameStates.CheckMate, game.State);
            Assert.NotEmpty(blackKingMovesPreviously);
            Assert.Empty(bKing.GetMovements());
        }

        [Fact]
        public void When_KingIsUnderAttack_Then_StateIsCheck()
        {
            List<Piece> pieces = new()
            {
                new Rook(PieceColor.Black, new BoardPosition("F8")),
                new Queen(PieceColor.Black, new BoardPosition("H7")),
                new Knight(PieceColor.White, new BoardPosition("G6")),
                new Pawn(PieceColor.White, new BoardPosition("B4")),
                new King(PieceColor.Black, new BoardPosition("D4")),
                new Queen(PieceColor.White, new BoardPosition("H3")),
                new Bishop(PieceColor.White, new BoardPosition("A2")),
                new Rook(PieceColor.White, new BoardPosition("D1")),
                new King(PieceColor.White, new BoardPosition("G1")),
            };

            ChessGame game = new(pieces);

            var bKing = game.Board.GetPiece(new BoardPosition("D4"));
            var wQueen = game.Board.GetPiece(new BoardPosition("H3"));

            var blackKingMovesPreviously = bKing.GetMovements().ToList();

            game.MovePiece(wQueen, new BoardPosition("G3"));

            Assert.Equal(GameStates.Check, game.State);
            Assert.NotEmpty(blackKingMovesPreviously);
            Assert.NotEmpty(bKing.GetMovements());
        }
    }
}
