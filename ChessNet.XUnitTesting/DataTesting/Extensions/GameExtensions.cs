using ChessNet.Data.Extensions;

namespace ChessNet.XUnitTesting.DataTesting.PieceMovements
{
    public class GameExtensions
    {
        [Fact]
        public void When_ColumnIsValidInt32_Then_ConvertToString()
        {
            int column1 = 0;
            int column8 = 7;
            int column26 = 25;
            int column27 = 26;
            int column418 = 417;
            int column703 = 702;
            int column702 = 701;
            int column782 = 781;

            string column1AsLetter = column1.ToColumnAnnotation();
            string column8AsLetter = column8.ToColumnAnnotation();
            string column26AsLetter = column26.ToColumnAnnotation();
            string column27AsLetter = column27.ToColumnAnnotation();
            string column418AsLetter = column418.ToColumnAnnotation();
            string column702AsLetter = column702.ToColumnAnnotation();
            string column703AsLetter = column703.ToColumnAnnotation();
            string column782AsLetter = column782.ToColumnAnnotation();

            Assert.Equal("A", column1AsLetter);
            Assert.Equal("H", column8AsLetter);
            Assert.Equal("Z", column26AsLetter);
            Assert.Equal("AA", column27AsLetter);
            Assert.Equal("PB", column418AsLetter);
            Assert.Equal("ZZ", column702AsLetter);
            Assert.Equal("AAA", column703AsLetter);
            Assert.Equal("ADB", column782AsLetter);
        }

        [Fact]
        public void When_ColumnIsValidString_Then_ConvertToInt32()
        {
            string column1AsLetter = "A";
            string column8AsLetter = "H";
            string column26AsLetter = "Z";
            string column27AsLetter = "AA";
            string column418AsLetter = "PB";
            string column702AsLetter = "ZZ";
            string column703AsLetter = "AAA";
            string column782AsLetter = "ADB";

            int column1 = column1AsLetter.ToColumnInteger();
            int column8 = column8AsLetter.ToColumnInteger();
            int column26 = column26AsLetter.ToColumnInteger();
            int column27 = column27AsLetter.ToColumnInteger();
            int column418 = column418AsLetter.ToColumnInteger();
            int column702 = column702AsLetter.ToColumnInteger();
            int column703 = column703AsLetter.ToColumnInteger();
            int column782 = column782AsLetter.ToColumnInteger();

            Assert.Equal(0, column1);
            Assert.Equal(7, column8);
            Assert.Equal(25, column26);
            Assert.Equal(26, column27);
            Assert.Equal(417, column418);
            Assert.Equal(702, column703);
            Assert.Equal(701, column702);
            Assert.Equal(781, column782);
        }
    }
}