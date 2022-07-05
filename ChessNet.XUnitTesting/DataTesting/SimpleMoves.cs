using ChessNet.Data.Enums;
using ChessNet.Data.Models;
using ChessNet.Data.Models.Pieces;
using ChessNet.Data.Structs;

namespace ChessNet.XUnitTesting.DataTesting
{
    public class SimpleMoves
    {
        [Fact]
        public void When_BoardIsInitialized_Then_MovePawn()
        {
            ChessGame game = new();

            var pawn = game.CurrentPlayer.Pieces.First(p => p is Pawn);
            var validMoves = pawn.GetMovements().ToList();
            var moveResult = game.MovePiece(pawn, validMoves.First().Destination);

            Assert.True(validMoves.Count() > 1);
            Assert.True(moveResult.IsValid);
        }

        [Fact]
        public void When_BoardIsInitializedToCapture_Then_MovePawnToCapture()
        {
            List<Piece> pieces = new()
            {
                new Pawn(PieceColor.White, new BoardPosition(4, 4)),
                new Pawn(PieceColor.Black, new BoardPosition(5, 5)),
            };

            ChessGame game = new(pieces);

            var startingPlayerColor = game.CurrentPlayer.Color;
            var previousCount = game.Board.PieceCount;
            var pawn = game.CurrentPlayer.Pieces.First(p => p is Pawn) as Pawn;
            var previousPosition = pawn.Position;
            var validMoves = pawn.GetMovements().ToList();
            var captureMove = validMoves.Where(m => m.IsCaptureFor(startingPlayerColor)).FirstOrDefault();
            var moveResult = game.MovePiece(pawn, captureMove.Destination);

            Assert.True(game.Board.PieceCount < previousCount);
            Assert.True(captureMove.IsCaptureFor(startingPlayerColor) && previousPosition != pawn.Position);
            Assert.True(validMoves.Count() > 1);
            Assert.True(moveResult.IsValid);
        }
    }
}