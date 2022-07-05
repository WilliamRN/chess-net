using ChessNet.Data.Enums;
using ChessNet.Data.Models.Pieces;
using ChessNet.Data.Structs;

namespace ChessNet.XUnitTesting.DataTesting.Structs
{
    public class PieceMovements
    {
        [Fact]
        public void When_PieceMovementIsDefinedToPosition_Then_NotDefault()
        {
            Movement pieceMovement1 = new(new BoardPosition());
            Movement pieceMovement2 = new(new BoardPosition(1, 1));
            Movement pieceMovement3 = new(new BoardPosition(1, 1), new Pawn(PieceColor.White, new BoardPosition(2, 2)));

            Assert.True(!pieceMovement1.IsDefault);
            Assert.True(!pieceMovement2.IsDefault);
            Assert.True(!pieceMovement3.IsDefault);
        }

        [Fact]
        public void When_PieceMovementIsNotDefined_Then_IsDefault()
        {
            Movement pieceMovement = default;

            Assert.True(pieceMovement.IsDefault);
        }
    }
}