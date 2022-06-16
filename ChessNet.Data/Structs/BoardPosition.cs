﻿namespace ChessNet.Data.Structs
{
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
    }
}