using ChessNet.Data.Enums;
using ChessNet.Data.Models.Pieces;
using ChessNet.Data.Structs;

namespace ChessNet.XUnitTesting.Structs
{
    public class BoardPositions
    {
        [Fact]
        public void When_BoardPositionIsValid_Then_NotDefault()
        {
            BoardPosition boardPosition1 = new(0, 0);
            BoardPosition boardPosition2 = new(5, 2);
            BoardPosition boardPosition3 = new(78, 2567);
            BoardPosition boardPosition4 = new("b4");
            BoardPosition boardPosition5 = new("4b");
            BoardPosition boardPosition6 = new("ZZ78");
            BoardPosition boardPosition7 = new("ú3");
            BoardPosition boardPosition8 = new("D 5");

            BoardPosition stepOffset1 = new(-1, 0);
            BoardPosition stepOffset2 = new(0, -1);

            Assert.True(!boardPosition1.IsDefault);
            Assert.True(!boardPosition2.IsDefault);
            Assert.True(!boardPosition3.IsDefault);
            Assert.True(!boardPosition4.IsDefault);
            Assert.True(!boardPosition5.IsDefault);
            Assert.True(!boardPosition6.IsDefault);
            Assert.True(!boardPosition7.IsDefault);
            Assert.True(!boardPosition8.IsDefault);

            Assert.True(!stepOffset1.IsDefault);
            Assert.True(!stepOffset2.IsDefault);
        }

        [Fact]
        public void When_BoardPositionIsNotDefined_Then_IsDefault()
        {
            BoardPosition boardPosition = new();

            Assert.True(boardPosition.IsDefault);
        }

        [Fact]
        public void When_BoardPositionIsInvalid_Then_ThrowExceptions()
        {
            Assert.Throws<InvalidOperationException>(() => {
                BoardPosition boardPosition = new("A");
            });

            Assert.Throws<InvalidOperationException>(() => {
                BoardPosition boardPosition = new("5");
            });

            Assert.Throws<InvalidOperationException>(() => {
                BoardPosition boardPosition = new("&##@@@!!!");
            });

            Assert.Throws<ArgumentNullException>(() => {
                BoardPosition boardPosition = new(" ");
            });

            Assert.Throws<ArgumentNullException>(() => {
                BoardPosition boardPosition = new("");
            });

            Assert.Throws<ArgumentNullException>(() => {
                BoardPosition boardPosition = new(null);
            });
        }
    }
}