using ChessNet.Data.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessNet.Desktop.Models.Events
{
    public class CellUpdateEvent
    {
        public BoardPosition Position { get; set; }

        public CellUpdateEvent(BoardPosition position)
        {
            Position = position;
        }
    }
}
