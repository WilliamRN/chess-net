using ChessNet.Data.Extensions;
using ChessNet.Data.Utils;
using System.Diagnostics;

namespace ChessNet.Data.Structs
{
    [DebuggerDisplay("Position: {this.AsString()}")]
    public struct BoardPosition
    {
        private readonly bool _isPopulated;

        public int Column;
        public int Row;

        public bool IsDefault => !_isPopulated;

        public BoardPosition(int column, int row)
        {
            Column = column;
            Row = row;

            _isPopulated = true;
        }

        public BoardPosition(string positionAnnotation)
        {
            if (string.IsNullOrWhiteSpace(positionAnnotation))
                throw new ArgumentNullException(nameof(positionAnnotation), "position cannot be empty");

            Column = positionAnnotation
                .GetLettersOnly()
                .ToColumnInteger();

            var isValidRow = int.TryParse(positionAnnotation.GetNumbersOnly(), out Row);
            Row -= 1;

            if (Column < 0)
                throw new InvalidOperationException("invalid column annotation");

            if (Row < 0 || !isValidRow)
                throw new InvalidOperationException("invalid row annotation");

            _isPopulated = true;
        }

        public BoardPosition GetOffset(int offsetColumBy, int offsetRowBy) =>
            new(Column + offsetColumBy, Row + offsetRowBy);

        public BoardPosition GetOffset(BoardPosition offset) =>
            GetOffset(offset.Column, offset.Row);

        public string AsString() =>
            !IsDefault ? $"{Column.ToColumnAnnotation()}{Row + 1}" : "?";

        public static bool operator ==(BoardPosition left, BoardPosition right) => 
            left.Column == right.Column && left.Row == right.Row;

        public static bool operator !=(BoardPosition left, BoardPosition right) => 
            left.Column != right.Column || left.Row != right.Row;

        public static BoardPosition operator +(BoardPosition left, BoardPosition right) =>
            new(left.Column + right.Column, left.Row + right.Row);

        public static BoardPosition operator -(BoardPosition left, BoardPosition right) => 
            new(left.Column - right.Column, left.Row - right.Row);

        public override bool Equals(object obj)
        {
            BoardPosition? objAsPosition = obj as BoardPosition?;

            if (objAsPosition == null) return false;
            else return objAsPosition.Value == this;
        }

        public override int GetHashCode() =>
            HashCode.Combine(Column, Row);
    }
}