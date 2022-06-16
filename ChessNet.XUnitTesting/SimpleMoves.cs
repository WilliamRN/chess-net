using ChessNet.Data.Enums;
using ChessNet.Data.Models;
using ChessNet.Data.Models.Pieces;
using ChessNet.Data.Structs;

namespace ChessNet.XUnitTesting
{
    public class SimpleMoves
    {
        [Fact]
        public void When_BoardIsInitialized_Then_MovePawn()
        {
            ChessGame game = new ChessGame();
            var pawn = game.CurrentPlayer.Pieces.First(p => p is Pawn) as Pawn;
            var validMoves = pawn.GetMovements(game.Board).ToList();

            bool isValidMove = game.MovePiece(pawn, validMoves.First().Destination);

            Assert.True(validMoves.Count() > 1);
            Assert.True(isValidMove);
        }

        [Fact]
        public void When_BoardIsInitializedToCapture_Then_MovePawnToCapture()
        {
            int previousCount, nextCount;
            List<Piece> pieces = new List<Piece>
            {
                new Pawn(PieceColor.White, new BoardPosition(4, 4)),
                new Pawn(PieceColor.Black, new BoardPosition(5, 5)),
            };

            ChessGame game = new ChessGame(pieces);
            previousCount = game.Board.PieceCount;
            var pawn = game.CurrentPlayer.Pieces.First(p => p is Pawn) as Pawn;
            var validMoves = pawn.GetMovements(game.Board).ToList();
            var captureMove = validMoves.Where(m => m.IsCapture).FirstOrDefault();

            bool isValidMove = game.MovePiece(pawn, captureMove.Destination);

            nextCount = game.Board.PieceCount;

            Assert.True(nextCount < previousCount);
            Assert.True(captureMove.IsCapture && captureMove.Destination.Column != pawn.Position.Column);
            Assert.True(validMoves.Count() > 1);
            Assert.True(isValidMove);
        }
    }
}