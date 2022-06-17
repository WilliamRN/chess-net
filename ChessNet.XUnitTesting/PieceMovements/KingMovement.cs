using ChessNet.Data.Enums;
using ChessNet.Data.Models;
using ChessNet.Data.Models.Pieces;
using ChessNet.Data.Structs;
using ChessNet.Data.Extensions;

namespace ChessNet.XUnitTesting.PieceMovements
{
    public class KingMovement
    {
        [Fact]
        public void When_MovingKing_Then_MovesAreValidated()
        {
            List<Piece> pieces = new()
            {
                new King(PieceColor.White, new BoardPosition(0, 4)),
                new Rook(PieceColor.White, new BoardPosition(0, 5)),
                new Rook(PieceColor.Black, new BoardPosition(1, 4)),
            };

            ChessGame game = new(pieces);

            var king = game.Board.GetPiece(0, 4);
            var movesAvailable = king.GetMovements();

            var isMoveToCaptureFriendValid = movesAvailable.TryMoveTo(0, 5, out PieceMovement moveToCaptureFriend);
            var isMoveToOutsideOfBoardValid = movesAvailable.TryMoveTo(-1, 4, out PieceMovement moveToOutsideOfBoard);
            var isMoveToOutsideOfRange = movesAvailable.TryMoveTo(0, 7, out PieceMovement moveToOutsideOfRange);
            var isMoveToCaptureEnemyValid = movesAvailable.TryMoveTo(1, 4, out PieceMovement moveToCaptureEnemy);
            var isMoveToEmptyPathValid = movesAvailable.TryMoveTo(0, 3, out PieceMovement moveToEmptyPath);
            var isMoveToEmptyDiagnalValid = movesAvailable.TryMoveTo(1, 5, out PieceMovement moveToEmptyDiagnal);

            Assert.True(!isMoveToCaptureFriendValid && moveToCaptureFriend.IsDefault);
            Assert.True(!isMoveToOutsideOfBoardValid && moveToOutsideOfBoard.IsDefault);
            Assert.True(!isMoveToOutsideOfRange && moveToOutsideOfRange.IsDefault);
            Assert.True(isMoveToCaptureEnemyValid && !moveToCaptureEnemy.IsDefault);
            Assert.True(isMoveToEmptyPathValid && !moveToEmptyPath.IsDefault);
            Assert.True(isMoveToEmptyDiagnalValid && !moveToEmptyDiagnal.IsDefault);
        }

        [Fact]
        public void When_KingIsSetToCapture_Then_Capture()
        {
            List<Piece> pieces = new()
            {
                new King(PieceColor.White, new BoardPosition(4, 4)),
                new Pawn(PieceColor.Black, new BoardPosition(4, 5)),
            };

            ChessGame game = new(pieces);

            var startingPlayerColor = game.CurrentPlayer.Color;
            var previousCount = game.Board.PieceCount;
            var king = game.CurrentPlayer.Pieces.First(p => p is King);
            var previousPosition = king.Position;
            var validMoves = king.GetMovements().ToList();
            var captureMove = validMoves.Where(m => m.IsCaptureFor(startingPlayerColor)).FirstOrDefault();
            var isValidMove = game.MovePiece(king, captureMove.Destination);

            Assert.True(game.Board.PieceCount < previousCount);
            Assert.True(captureMove.IsCaptureFor(startingPlayerColor) && previousPosition != king.Position);
            Assert.True(validMoves.Count() > 1);
            Assert.True(isValidMove);
        }
    }
}