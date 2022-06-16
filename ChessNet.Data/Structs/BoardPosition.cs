using ChessNet.Data.Extensions;
using System.Diagnostics;

namespace ChessNet.Data.Structs
{
    [DebuggerDisplay("Position: {this.AsString()}")]
    public struct BoardPosition
    {
        public int Column;
        public int Row;

        public BoardPosition(int column, int row)
        {
            Column = column;
            Row = row;
        }

        public BoardPosition GetOffset(int offsetColumBy, int offsetRowBy)
        {
            return new BoardPosition(Column + offsetColumBy, Row + offsetRowBy);
        }

        public BoardPosition GetOffset(BoardPosition offset) =>
            GetOffset(offset.Column, offset.Row);

        public string AsString()
        {
            return $"{Column.AsLetter()}{Row + 1}";
        }

        public static bool operator ==(BoardPosition left, BoardPosition right)
        {
            return left.Column == right.Column && left.Row == right.Row;
        }

        public static bool operator !=(BoardPosition left, BoardPosition right)
        {
            return left.Column != right.Column || left.Row != right.Row;
        }

        public override bool Equals(object obj)
        {
            BoardPosition? objAsPosition = obj as BoardPosition?;

            if (objAsPosition == null) return false;
            else return objAsPosition.Value == this;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Column, Row);
        }
    }
}