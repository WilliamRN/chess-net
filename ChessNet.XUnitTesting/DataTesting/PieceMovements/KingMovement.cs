using ChessNet.Data.Enums;
using ChessNet.Data.Models;
using ChessNet.Data.Models.Pieces;
using ChessNet.Data.Structs;
using ChessNet.Data.Extensions;

namespace ChessNet.XUnitTesting.DataTesting.PieceMovements
{
    public class KingMovement
    {
        [Fact]
        public void When_MovingKing_Then_MovesAreValidated()
        {
            List<Piece> pieces = new()
            {
                new King(PieceColor.White, new BoardPosition("A5")),
                new Rook(PieceColor.White, new BoardPosition("B6")),
                new Pawn(PieceColor.Black, new BoardPosition("B5")),
            };

            ChessGame game = new(pieces);

            var king = game.Board.GetPiece(0, 4);
            var movesAvailable = king.GetMovements().ToList();

            var isMoveToCaptureFriendValid = movesAvailable.TryMoveTo(new BoardPosition("B6"), out Movement moveToCaptureFriend);
            var isMoveToOutsideOfBoardValid = movesAvailable.TryMoveTo(new BoardPosition(-1, 4), out Movement moveToOutsideOfBoard);
            var isMoveToOutsideOfRange = movesAvailable.TryMoveTo(new BoardPosition("A1"), out Movement moveToOutsideOfRange);
            var isMoveToCaptureEnemyValid = movesAvailable.TryMoveTo(new BoardPosition("B5"), out Movement moveToCaptureEnemy);
            var isMoveToEmptyPathValid = movesAvailable.TryMoveTo(new BoardPosition("A6"), out Movement moveToEmptyPath);
            var isMoveToEmptyDiagnalValid = movesAvailable.TryMoveTo(new BoardPosition("B4"), out Movement moveToEmptyDiagnal);

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
            var moveResult = game.MovePiece(king, captureMove.Destination);

            Assert.True(game.Board.PieceCount < previousCount);
            Assert.True(captureMove.IsCaptureFor(startingPlayerColor) && previousPosition != king.Position);
            Assert.True(validMoves.Count() > 1);
            Assert.True(moveResult.IsValid);
        }

        [Fact]
        public void When_KingIsSetToCastling_Then_DoCastling()
        {
            List<Piece> pieces = new()
            {
                new King(PieceColor.White, new BoardPosition("E1")),
                new Rook(PieceColor.White, new BoardPosition("A1")),
                new King(PieceColor.Black, new BoardPosition("E8")),
            };

            ChessGame game = new(pieces);

            var king = game.CurrentPlayer.Pieces.First(p => p is King);
            var rook = game.CurrentPlayer.Pieces.First(p => p is Rook);
            var previousPostion = king.Position;

            var validMoves = king.GetMovements().ToList();
            var validCastlings = validMoves.Where(m => m.IsCastling).Count();
            var moveResult = game.MovePiece(king, validMoves.First(m => m.IsCastling).Destination);

            Assert.Equal(new BoardPosition("C1"), king.Position);
            Assert.Equal(new BoardPosition("D1"), rook.Position);
            Assert.True(validCastlings == 1);
            Assert.True(moveResult.IsValid);
        }

        [Fact]
        public void When_KingIsInCheck_Then_NoValidCastlingIsAvailable()
        {
            List<Piece> pieces = new()
            {
                new King(PieceColor.White, new BoardPosition("E1")),
                new Rook(PieceColor.White, new BoardPosition("A1")),
                new Queen(PieceColor.Black, new BoardPosition("B4")),
                new King(PieceColor.Black, new BoardPosition("E8")),
            };

            ChessGame game = new(pieces);

            var king = game.CurrentPlayer.Pieces.First(p => p is King);
            var rook = game.CurrentPlayer.Pieces.First(p => p is Rook);
            var previousPostion = king.Position;

            var validMoves = king.GetMovements().ToList();
            var validCastlings = validMoves.Where(m => m.IsCastling).Count();

            Assert.Equal(new BoardPosition("E1"), king.Position);
            Assert.Equal(new BoardPosition("A1"), rook.Position);
            Assert.True(validCastlings == 0);
        }

        [Fact]
        public void When_KingMoveIsUnderAttack_Then_CannotMove()
        {
            List<Piece> pieces = new()
            {
                new King(PieceColor.White, new BoardPosition("E1")),
                new Queen(PieceColor.Black, new BoardPosition("B4")),
                new King(PieceColor.Black, new BoardPosition("E8")),
            };

            ChessGame game = new(pieces);

            var king = game.CurrentPlayer.Pieces.First(p => p is King);

            var moveResult = game.MovePiece(king, new BoardPosition("D2"));

            Assert.True(!moveResult.IsValid);
            Assert.Equal(new BoardPosition("E1"), king.Position);
        }

        [Fact]
        public void When_KingIsUnderAttackInAllMoves_Then_HasNoMoves()
        {
            List<Piece> pieces = new()
            {
                new King(PieceColor.White, new BoardPosition("D4")),
                new Knight(PieceColor.Black, new BoardPosition("G6")),
                new Pawn(PieceColor.Black, new BoardPosition("B6")),
                new Queen(PieceColor.Black, new BoardPosition("F3")),
                new Rook(PieceColor.Black, new BoardPosition("D1")),
                new Bishop(PieceColor.Black, new BoardPosition("A2")),
            };

            ChessGame game = new(pieces);

            var king = game.CurrentPlayer.Pieces.First(p => p is King);
            var validMoves = king.GetMovements().ToList();

            Assert.Empty(validMoves);
        }
    }
}