using ChessNet.Data.Enums;
using ChessNet.Data.Models;
using ChessNet.Data.Models.Pieces;
using ChessNet.Data.Structs;
using ChessNet.Data.Extensions;

namespace ChessNet.XUnitTesting.DataTesting.PieceMovements
{
    public class BishopMovement
    {
        [Fact]
        public void When_MovingBishop_Then_MovesAreValidated()
        {
            List<Piece> pieces = new()
            {
                new Bishop(PieceColor.White, new BoardPosition(3, 3)),
                new Rook(PieceColor.White, new BoardPosition(5, 5)),
                new Rook(PieceColor.Black, new BoardPosition(1, 5)),
            };

            ChessGame game = new(pieces);

            var bishop = game.Board.GetPiece(3, 3);
            var movesAvailable = bishop.GetMovements();

            var isMoveToCaptureFriendValid = movesAvailable.TryMoveTo(5, 5, out Movement moveToCaptureFriend);
            var isMoveToOutsideOfBoardValid = movesAvailable.TryMoveTo(8, 8, out Movement moveToOutsideOfBoard);
            var isMoveToOutsideOfRange = movesAvailable.TryMoveTo(6, 6, out Movement moveToOutsideOfRange);
            var isMoveToCaptureEnemyValid = movesAvailable.TryMoveTo(1, 5, out Movement moveToCaptureEnemy);
            var isMoveToEmptyPathValid = movesAvailable.TryMoveTo(5, 1, out Movement moveToEmptyPath);

            Assert.True(!isMoveToCaptureFriendValid && moveToCaptureFriend.IsDefault);
            Assert.True(!isMoveToOutsideOfBoardValid && moveToOutsideOfBoard.IsDefault);
            Assert.True(!isMoveToOutsideOfRange && moveToOutsideOfRange.IsDefault);
            Assert.True(isMoveToCaptureEnemyValid && !moveToCaptureEnemy.IsDefault);
            Assert.True(isMoveToEmptyPathValid && !moveToEmptyPath.IsDefault);
        }

        [Fact]
        public void When_BishopIsSetToCapture_Then_Capture()
        {
            List<Piece> pieces = new()
            {
                new Bishop(PieceColor.White, new BoardPosition(3, 3)),
                new Rook(PieceColor.White, new BoardPosition(5, 5)),
                new Rook(PieceColor.Black, new BoardPosition(1, 5)),
            };

            ChessGame game = new(pieces);

            var startingPlayerColor = game.CurrentPlayer.Color;
            var previousCount = game.Board.PieceCount;
            var bishop = game.CurrentPlayer.Pieces.First(p => p is Bishop);
            var previousPosition = bishop.Position;
            var validMoves = bishop.GetMovements().ToList();
            var captureMove = validMoves.Where(m => m.IsCaptureFor(startingPlayerColor)).FirstOrDefault();
            var isValidMove = game.MovePiece(bishop, captureMove.Destination);

            Assert.True(game.Board.PieceCount < previousCount);
            Assert.True(captureMove.IsCaptureFor(startingPlayerColor) && previousPosition != bishop.Position);
            Assert.True(validMoves.Count() > 1);
            Assert.True(isValidMove);
        }
    }
}