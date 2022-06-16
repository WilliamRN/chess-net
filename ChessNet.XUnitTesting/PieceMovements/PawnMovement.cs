using ChessNet.Data.Enums;
using ChessNet.Data.Models;
using ChessNet.Data.Models.Pieces;
using ChessNet.Data.Structs;
using ChessNet.Data.Extensions;

namespace ChessNet.XUnitTesting.PieceMovements
{
    public class PawnMovement
    {
        [Fact]
        public void When_MovingPawn_Then_MovesAreValidated()
        {
            List<Piece> pieces = new()
            {
                new Pawn(PieceColor.White, new BoardPosition(4, 4)),
                new Pawn(PieceColor.White, new BoardPosition(4, 6)),
                new Pawn(PieceColor.White, new BoardPosition(5, 5)),
                new Pawn(PieceColor.Black, new BoardPosition(3, 5)),
            };

            ChessGame game = new(pieces);

            var pawn = game.Board.GetPiece(4, 4);
            var movesAvailable = pawn.GetMovements(game.Board);

            var isMoveToCaptureFriendValid = movesAvailable.TryMoveTo(5, 5, out PieceMovement moveToCaptureFriend);
            var isMoveToOutsideOfBoardValid = movesAvailable.TryMoveTo(4, 9, out PieceMovement moveToOutsideOfBoard);
            var isMoveToOutsideOfRangeValid = movesAvailable.TryMoveTo(4, 6, out PieceMovement moveToOutsideOfRange);
            var isMoveToCaptureEnemyValid = movesAvailable.TryMoveTo(3, 5, out PieceMovement moveToCaptureEnemy);
            var isMoveToEmptyPathValid = movesAvailable.TryMoveTo(4, 5, out PieceMovement moveToEmptyPath);

            Assert.True(!isMoveToCaptureFriendValid && moveToCaptureFriend.IsDefault);
            Assert.True(!isMoveToOutsideOfBoardValid && moveToOutsideOfBoard.IsDefault);
            Assert.True(!isMoveToOutsideOfRangeValid && moveToOutsideOfRange.IsDefault);
            Assert.True(isMoveToCaptureEnemyValid && !moveToCaptureEnemy.IsDefault);
            Assert.True(isMoveToEmptyPathValid && !moveToEmptyPath.IsDefault);
        }

        [Fact]
        public void When_PawnIsSetToCapture_Then_Capture()
        {
            List<Piece> pieces = new()
            {
                new Pawn(PieceColor.White, new BoardPosition(4, 4)),
                new Pawn(PieceColor.Black, new BoardPosition(3, 5)),
            };

            ChessGame game = new(pieces);

            var startingPlayerColor = game.CurrentPlayer.Color;
            var previousCount = game.Board.PieceCount;
            var pawn = game.CurrentPlayer.Pieces.First(p => p is Pawn);
            var previousPosition = pawn.Position;
            var validMoves = pawn.GetMovements(game.Board).ToList();
            var captureMove = validMoves.Where(m => m.IsCaptureFor(startingPlayerColor)).FirstOrDefault();
            var isValidMove = game.MovePiece(pawn, captureMove.Destination);

            Assert.True(game.Board.PieceCount < previousCount);
            Assert.True(captureMove.IsCaptureFor(startingPlayerColor) && previousPosition != pawn.Position);
            Assert.True(validMoves.Count > 1);
            Assert.True(isValidMove);
        }
    }
}