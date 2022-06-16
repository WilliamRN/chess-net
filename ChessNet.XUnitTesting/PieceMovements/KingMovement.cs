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
                new King(PieceColor.White, new BoardPosition(4, 4)),
                new Rook(PieceColor.White, new BoardPosition(4, 5)),
                new Rook(PieceColor.Black, new BoardPosition(5, 4)),
            };

            ChessGame game = new(pieces);

            var king = game.Board.GetPiece(4, 4);
            var movesAvailable = king.GetMovements(game.Board);

            var isMoveToCaptureFriendValid = movesAvailable.TryMoveTo(4, 5, out PieceMovement moveToCaptureFriend);
            var isMoveToCaptureEnemyValid = movesAvailable.TryMoveTo(5, 4, out PieceMovement moveToCaptureEnemy);
            var isMoveToEmptyPathValid = movesAvailable.TryMoveTo(4, 3, out PieceMovement moveToEmptyPath);

            Assert.True(!isMoveToCaptureFriendValid && moveToCaptureFriend.IsDefault);
            Assert.True(isMoveToCaptureEnemyValid && !moveToCaptureEnemy.IsDefault);
            Assert.True(isMoveToEmptyPathValid && !moveToEmptyPath.IsDefault);
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
            var validMoves = king.GetMovements(game.Board).ToList();
            var captureMove = validMoves.Where(m => m.IsCaptureFor(startingPlayerColor)).FirstOrDefault();
            var isValidMove = game.MovePiece(king, captureMove.Destination);

            Assert.True(game.Board.PieceCount < previousCount);
            Assert.True(captureMove.IsCaptureFor(startingPlayerColor) && previousPosition != king.Position);
            Assert.True(validMoves.Count > 1);
            Assert.True(isValidMove);
        }
    }
}