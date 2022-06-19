using ChessNet.Data.Enums;
using ChessNet.Data.Models;
using ChessNet.Data.Models.Pieces;
using ChessNet.Data.Structs;
using ChessNet.Data.Extensions;

namespace ChessNet.XUnitTesting.DataTesting.PieceMovements
{
    public class QueenMovement
    {
        [Fact]
        public void When_MovingQueen_Then_MovesAreValidated()
        {
            List<Piece> pieces = new()
            {
                new Queen(PieceColor.White, new BoardPosition(4, 4)),
                new Queen(PieceColor.White, new BoardPosition(6, 6)),
                new Queen(PieceColor.Black, new BoardPosition(1, 4)),
            };

            ChessGame game = new(pieces);

            var queen = game.Board.GetPiece(4, 4);
            var movesAvailable = queen.GetMovements();

            var isMoveToCaptureFriendValid = movesAvailable.TryMoveTo(6, 6, out Movement moveToCaptureFriend);
            var isMoveToOutsideOfBoardValid = movesAvailable.TryMoveTo(-1, 4, out Movement moveToOutsideOfBoard);
            var isMoveToOutsideOfRange = movesAvailable.TryMoveTo(7, 7, out Movement moveToOutsideOfRange);
            var isMoveToCaptureEnemyValid = movesAvailable.TryMoveTo(1, 4, out Movement moveToCaptureEnemy);
            var isMoveToEmptyPathValid = movesAvailable.TryMoveTo(4, 1, out Movement moveToEmptyPath);
            var isMoveToEmptyDiagnalValid = movesAvailable.TryMoveTo(1, 1, out Movement moveToEmptyDiagnal);

            Assert.True(!isMoveToCaptureFriendValid && moveToCaptureFriend.IsDefault);
            Assert.True(!isMoveToOutsideOfBoardValid && moveToOutsideOfBoard.IsDefault);
            Assert.True(!isMoveToOutsideOfRange && moveToOutsideOfRange.IsDefault);
            Assert.True(isMoveToCaptureEnemyValid && !moveToCaptureEnemy.IsDefault);
            Assert.True(isMoveToEmptyPathValid && !moveToEmptyPath.IsDefault);
            Assert.True(isMoveToEmptyDiagnalValid && !moveToEmptyDiagnal.IsDefault);
        }

        [Fact]
        public void When_QueenIsSetToCapture_Then_Capture()
        {
            List<Piece> pieces = new()
            {
                new Queen(PieceColor.White, new BoardPosition(4, 4)),
                new Queen(PieceColor.Black, new BoardPosition(1, 4)),
            };

            ChessGame game = new(pieces);

            var startingPlayerColor = game.CurrentPlayer.Color;
            var previousCount = game.Board.PieceCount;
            var queen = game.CurrentPlayer.Pieces.First(p => p is Queen);
            var previousPosition = queen.Position;
            var validMoves = queen.GetMovements().ToList();
            var captureMove = validMoves.Where(m => m.IsCaptureFor(startingPlayerColor)).FirstOrDefault();
            var isValidMove = game.MovePiece(queen, captureMove.Destination);

            Assert.True(game.Board.PieceCount < previousCount);
            Assert.True(captureMove.IsCaptureFor(startingPlayerColor) && previousPosition != queen.Position);
            Assert.True(validMoves.Count() > 1);
            Assert.True(isValidMove);
        }
    }
}