using ChessNet.Data.Enums;
using ChessNet.Data.Models;
using ChessNet.Data.Models.Pieces;
using ChessNet.Data.Structs;

namespace ChessNet.XUnitTesting
{
    public class Structs
    {
        [Fact]
        public void When_PieceMovementIsDefinedToPosition_Then_NotDefault()
        {
            PieceMovement pieceMovement1 = new(new BoardPosition());
            PieceMovement pieceMovement2 = new(new BoardPosition(1, 1));
            PieceMovement pieceMovement3 = new(new BoardPosition(1, 1), new Pawn(PieceColor.White, new BoardPosition(2, 2)));

            Assert.True(!pieceMovement1.IsDefault);
            Assert.True(!pieceMovement2.IsDefault);
            Assert.True(!pieceMovement3.IsDefault);
        }

        [Fact]
        public void When_PieceMovementIsNotDefined_Then_IsDefault()
        {
            PieceMovement pieceMovement = new();

            Assert.True(pieceMovement.IsDefault);
        }
    }
}