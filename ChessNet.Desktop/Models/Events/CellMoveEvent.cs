using ChessNet.Data.Enums;
using ChessNet.Data.Models;
using ChessNet.Data.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessNet.Desktop.Models.Events
{
    public class CellMoveEvent
    {
        public Player Player { get; set; }
        public BoardPosition From { get; set; }
        public BoardPosition To { get; set; }

        public CellMoveEvent(Player player, BoardPosition from, BoardPosition to)
        {
            Player = player;
            From = from;
            To = to;
        }
    }
}
