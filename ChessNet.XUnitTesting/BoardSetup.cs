using ChessNet.Data.Enums;
using ChessNet.Data.Models;
using ChessNet.Data.Models.Pieces;
using ChessNet.Data.Structs;

namespace ChessNet.XUnitTesting
{
    public class BoardSetup
    {
        [Fact]
        public void When_BoardIsInitializedWithInvalidPiecePosition_Then_RaiseInvalidOperationException()
        {
            List<Piece> pieces = new()
            {
                new Pawn(PieceColor.White, new BoardPosition(0, 0)),
                new Pawn(PieceColor.White, new BoardPosition(0, 0)),
            };

            Assert.Throws<InvalidOperationException>(() =>
            {
                ChessGame game = new(pieces);
            });
        }

        [Fact]
        public void When_BoardIsInitialized_Then_PrintBoard()
        {
            ChessGame game = new();

            string board = game.Board.PrintBoard();

            Assert.True(!string.IsNullOrEmpty(board));
        }
    }
}