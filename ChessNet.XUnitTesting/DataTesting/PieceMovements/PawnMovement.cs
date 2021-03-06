using ChessNet.Data.Enums;
using ChessNet.Data.Models;
using ChessNet.Data.Models.Pieces;
using ChessNet.Data.Structs;
using ChessNet.Data.Extensions;

namespace ChessNet.XUnitTesting.DataTesting.PieceMovements
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
            var movesAvailable = pawn.GetMovements();

            var isMoveToCaptureFriendValid = movesAvailable.TryMoveTo(5, 5, out Movement moveToCaptureFriend);
            var isMoveToOutsideOfBoardValid = movesAvailable.TryMoveTo(4, 9, out Movement moveToOutsideOfBoard);
            var isMoveToOutsideOfRangeValid = movesAvailable.TryMoveTo(4, 6, out Movement moveToOutsideOfRange);
            var isMoveToCaptureEnemyValid = movesAvailable.TryMoveTo(3, 5, out Movement moveToCaptureEnemy);
            var isMoveToEmptyPathValid = movesAvailable.TryMoveTo(4, 5, out Movement moveToEmptyPath);

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
            var validMoves = pawn.GetMovements().ToList();
            var captureMove = validMoves.Where(m => m.IsCaptureFor(startingPlayerColor)).FirstOrDefault();
            var moveResult = game.MovePiece(pawn, captureMove.Destination);

            Assert.True(game.Board.PieceCount < previousCount);
            Assert.True(captureMove.IsCaptureFor(startingPlayerColor) && previousPosition != pawn.Position);
            Assert.True(validMoves.Count() > 1);
            Assert.True(moveResult.IsValid);
        }

        [Fact]
        public void When_PawnIsSetToEnPassant_Then_Capture()
        {
            List<Piece> pieces = new()
            {
                new King(PieceColor.White, new BoardPosition(4, 0)),
                new King(PieceColor.Black, new BoardPosition(4, 7)),

                new Pawn(PieceColor.White, new BoardPosition("E4")),
                new Pawn(PieceColor.Black, new BoardPosition("F7")),
            };

            ChessGame game = new(pieces);
            var previousCount = game.Board.PieceCount;

            // White Pawn has moved to the spot where the black Pawn must move two spaces to.
            var whitePawn = game.CurrentPlayer.Pieces.First(p => p is Pawn);
            game.MovePiece(whitePawn, new BoardPosition("E5"));

            // Black Pawn moves two spaces from starting position.
            var blackPawn = game.CurrentPlayer.Pieces.First(p => p is Pawn);
            game.MovePiece(blackPawn, new BoardPosition("F5"));

            // List of valid moves should include en passant.
            var validMoves = whitePawn.GetMovements().ToList();

            var isEnPassantAvailable = validMoves.Any(m => m.IsEnPassant);
            var enPassantMoveResult = game.MovePiece(whitePawn, validMoves.First(m => m.IsEnPassant).Destination);

            Assert.True(isEnPassantAvailable);
            Assert.True(validMoves.Count() > 1);
            Assert.True(isEnPassantAvailable);
            Assert.True(enPassantMoveResult.IsValid);
            Assert.Equal(new BoardPosition("F6"), whitePawn.Position);
        }

        [Fact]
        public void When_PawnReachesEdgeOfBoard_Then_PromotoToQueen()
        {
            List<Piece> pieces = new()
            {
                new King(PieceColor.White, new BoardPosition("E1")),
                new King(PieceColor.Black, new BoardPosition("G2")),

                new Pawn(PieceColor.White, new BoardPosition("B7")),
                new Pawn(PieceColor.Black, new BoardPosition("A7")),
            };

            ChessGame game = new(pieces);
            var startCount = game.Board.PieceCount;

            var whitePieceAtStart = game.CurrentPlayer.Pieces.First(p => p is Pawn);
            game.MovePiece(whitePieceAtStart, new BoardPosition("B8"));

            var blackPawn = game.CurrentPlayer.Pieces.First(p => p is Pawn);
            game.MovePiece(blackPawn, blackPawn.GetMovements().First().Destination);

            var whitePieceAtEnd = game.CurrentPlayer.Pieces.First(p => p is Queen);

            var queenMoves = whitePieceAtEnd.GetMovements().ToList();
            var endCount = game.Board.PieceCount;

            Assert.True(startCount == 4);
            Assert.True(whitePieceAtStart is Pawn);
            Assert.True(queenMoves.Count() > 7);
            Assert.True(whitePieceAtEnd is Queen);
            Assert.True(endCount == 4);
        }
    }
}