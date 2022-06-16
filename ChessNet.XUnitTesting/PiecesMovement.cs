using ChessNet.Data.Enums;
using ChessNet.Data.Models;
using ChessNet.Data.Models.Pieces;
using ChessNet.Data.Structs;

namespace ChessNet.XUnitTesting
{
    public class RookMovement
    {
        [Fact]
        public void When_RookIsSetToCapture_Then_Capture()
        {
            List<Piece> pieces = new List<Piece>
            {
                new Rook(PieceColor.White, new BoardPosition(0, 0)),
                new Pawn(PieceColor.Black, new BoardPosition(0, 6)),
            };

            ChessGame game = new ChessGame(pieces);

            var previousCount = game.Board.PieceCount;
            var rook = game.CurrentPlayer.Pieces.First(p => p is Rook) as Rook;
            var previousPosition = rook.Position;
            var validMoves = rook.GetMovements(game.Board).ToList();
            var captureMove = validMoves.Where(m => m.IsCapture).FirstOrDefault();
            var isValidMove = game.MovePiece(rook, captureMove.Destination);

            Assert.True(game.Board.PieceCount < previousCount);
            Assert.True(captureMove.IsCapture && previousPosition != rook.Position);
            Assert.True(validMoves.Count() > 1);
            Assert.True(isValidMove);
        }

        [Fact]
        public void When_RookIsMovedThenSetToCapture_Then_Capture()
        {
            // ARRANGE: Create board, with rook on offset column from pawn.
            List<Piece> pieces = new List<Piece>
            {
                new Rook(PieceColor.White, new BoardPosition(2, 0)),
                new Pawn(PieceColor.Black, new BoardPosition(0, 6)),
            };

            ChessGame game = new ChessGame(pieces);

            // ACT:
            var previousCount = game.Board.PieceCount;
            var rook = game.CurrentPlayer.Pieces.First(p => p is Rook) as Rook;

            // Ensure rook cannot capture black pawn.
            var isSetToCaptureBeforeMove = rook
                .GetMovements(game.Board)
                .Where(m => m.IsCapture)
                .Any();

            // Move rook to capture.
            game.MovePiece(rook, new BoardPosition(0, 0));

            // Move black pawn ahead.
            var blackPawn = game.CurrentPlayer.Pieces.First(p => p is Pawn) as Pawn;

            game.MovePiece(blackPawn, blackPawn.GetMovements(game.Board).First().Destination);

            // Capture with white rook.
            var rookAfterMove = game.CurrentPlayer.Pieces.First(p => p is Rook) as Rook;

            var captureMove = rookAfterMove
                .GetMovements(game.Board)
                .Where(m => m.IsCapture)
                .FirstOrDefault();

            var isValidMove = game.MovePiece(rook, captureMove.Destination);

            // ASSERT
            Assert.True(!isSetToCaptureBeforeMove);
            Assert.True(game.Board.PieceCount < previousCount);
            Assert.True(!blackPawn.IsWhite);
            Assert.True(isValidMove);
        }
    }
}