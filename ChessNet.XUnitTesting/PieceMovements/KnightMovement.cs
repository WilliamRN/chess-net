using ChessNet.Data.Enums;
using ChessNet.Data.Models;
using ChessNet.Data.Models.Pieces;
using ChessNet.Data.Structs;
using ChessNet.Data.Extensions;

namespace ChessNet.XUnitTesting.PieceMovements
{
    public class KnightMovement
    {
        [Fact]
        public void When_MovingKnight_Then_MovesAreValidated()
        {
            List<Piece> pieces = new()
            {
                new Knight(PieceColor.White, new BoardPosition(3, 4)),
                new Rook(PieceColor.White, new BoardPosition(4, 2)),
                new Rook(PieceColor.Black, new BoardPosition(5, 3)),
            };

            ChessGame game = new(pieces);

            var knight = game.Board.GetPiece(3, 4);
            var movesAvailable = knight.GetMovements();

            var isMoveToCaptureFriendValid = movesAvailable.TryMoveTo(4, 2, out PieceMovement moveToCaptureFriend);
            var isMoveToOutsideOfRange = movesAvailable.TryMoveTo(7, 7, out PieceMovement moveToOutsideOfRange);
            var isMoveToCaptureEnemyValid = movesAvailable.TryMoveTo(5, 3, out PieceMovement moveToCaptureEnemy);
            var isMoveToEmptyPathValid = movesAvailable.TryMoveTo(2, 2, out PieceMovement moveToEmptyPath);

            Assert.True(!isMoveToCaptureFriendValid && moveToCaptureFriend.IsDefault);
            Assert.True(!isMoveToOutsideOfRange && moveToOutsideOfRange.IsDefault);
            Assert.True(isMoveToCaptureEnemyValid && !moveToCaptureEnemy.IsDefault);
            Assert.True(isMoveToEmptyPathValid && !moveToEmptyPath.IsDefault);
        }

        [Fact]
        public void When_KnightIsSetToCapture_Then_Capture()
        {
            List<Piece> pieces = new()
            {
                new Knight(PieceColor.White, new BoardPosition(3, 4)),
                new Rook(PieceColor.Black, new BoardPosition(5, 3)),
            };

            ChessGame game = new(pieces);

            var startingPlayerColor = game.CurrentPlayer.Color;
            var previousCount = game.Board.PieceCount;
            var knight = game.CurrentPlayer.Pieces.First(p => p is Knight);
            var previousPosition = knight.Position;
            var validMoves = knight.GetMovements().ToList();
            var captureMove = validMoves.Where(m => m.IsCaptureFor(startingPlayerColor)).FirstOrDefault();
            var isValidMove = game.MovePiece(knight, captureMove.Destination);

            Assert.True(game.Board.PieceCount < previousCount);
            Assert.True(captureMove.IsCaptureFor(startingPlayerColor) && previousPosition != knight.Position);
            Assert.True(validMoves.Count() > 1);
            Assert.True(isValidMove);
        }
    }
}