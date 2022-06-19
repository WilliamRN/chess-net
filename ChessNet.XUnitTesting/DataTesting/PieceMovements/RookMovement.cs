using ChessNet.Data.Enums;
using ChessNet.Data.Models;
using ChessNet.Data.Models.Pieces;
using ChessNet.Data.Structs;
using ChessNet.Data.Extensions;

namespace ChessNet.XUnitTesting.DataTesting.PieceMovements
{
    public class RookMovement
    {
        [Fact]
        public void When_MovingRook_Then_MovesAreValidated()
        {
            List<Piece> pieces = new()
            {
                new Rook(PieceColor.White, new BoardPosition(4, 4)),
                new Rook(PieceColor.White, new BoardPosition(4, 5)),
                new Rook(PieceColor.Black, new BoardPosition(5, 4)),
            };

            ChessGame game = new(pieces);

            var rook = game.Board.GetPiece(4, 4);
            var movesAvailable = rook.GetMovements();

            var isMovePastFriendPieceValid = movesAvailable.TryMoveTo(4, 6, out Movement movePastFriendPiece);
            var isMovePastEnemyPieceValid = movesAvailable.TryMoveTo(6, 4, out Movement movePastEnemyPiece);
            var isMoveToCaptureFriendValid = movesAvailable.TryMoveTo(4, 5, out Movement moveToCaptureFriend);
            var isMoveToCaptureEnemyValid = movesAvailable.TryMoveTo(5, 4, out Movement moveToCaptureEnemy);
            var isMoveToEmptyPathValid = movesAvailable.TryMoveTo(4, 2, out Movement moveToEmptyPath);

            Assert.True(!isMovePastFriendPieceValid && movePastFriendPiece.IsDefault);
            Assert.True(!isMovePastEnemyPieceValid && movePastEnemyPiece.IsDefault);
            Assert.True(!isMoveToCaptureFriendValid && moveToCaptureFriend.IsDefault);
            Assert.True(isMoveToCaptureEnemyValid && !moveToCaptureEnemy.IsDefault);
            Assert.True(isMoveToEmptyPathValid && !moveToEmptyPath.IsDefault);
        }

        [Fact]
        public void When_RookIsSetToCapture_Then_Capture()
        {
            List<Piece> pieces = new()
            {
                new Rook(PieceColor.White, new BoardPosition(0, 0)),
                new Pawn(PieceColor.Black, new BoardPosition(0, 6)),
            };

            ChessGame game = new(pieces);

            var startingPlayerColor = game.CurrentPlayer.Color;
            var previousCount = game.Board.PieceCount;
            var rook = game.CurrentPlayer.Pieces.First(p => p is Rook) as Rook;
            var previousPosition = rook.Position;
            var validMoves = rook.GetMovements().ToList();
            var captureMove = validMoves.Where(m => m.IsCaptureFor(startingPlayerColor)).FirstOrDefault();
            var isValidMove = game.MovePiece(rook, captureMove.Destination);

            Assert.True(game.Board.PieceCount < previousCount);
            Assert.True(captureMove.IsCaptureFor(startingPlayerColor) && previousPosition != rook.Position);
            Assert.True(validMoves.Count() > 1);
            Assert.True(isValidMove);
        }

        [Fact]
        public void When_RookIsMovedThenSetToCapture_Then_Capture()
        {
            // ARRANGE: Create board, with rook on offset column from pawn.
            List<Piece> pieces = new()
            {
                new Rook(PieceColor.White, new BoardPosition(2, 0)),
                new Pawn(PieceColor.Black, new BoardPosition(0, 6)),
            };

            ChessGame game = new(pieces);

            // ACT:
            var previousCount = game.Board.PieceCount;
            var rook = game.CurrentPlayer.Pieces.First(p => p is Rook) as Rook;

            // Ensure white rook cannot capture black pawn.
            var isSetToCaptureBeforeMove = rook
                .GetMovements()
                .Where(m => m.IsCaptureFor(PieceColor.White))
                .Any();

            // Move rook to capture.
            game.MovePiece(rook, new BoardPosition(0, 0));

            // Move black pawn ahead.
            var blackPawn = game.CurrentPlayer.Pieces.First(p => p is Pawn) as Pawn;

            game.MovePiece(blackPawn, blackPawn.GetMovements().First().Destination);

            // Capture with white rook.
            var rookAfterMove = game.CurrentPlayer.Pieces.First(p => p is Rook) as Rook;

            var captureMove = rookAfterMove
                .GetMovements()
                .Where(m => m.IsCaptureFor(PieceColor.White))
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