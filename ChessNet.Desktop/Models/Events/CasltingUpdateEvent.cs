using ChessNet.Data.Enums;
using ChessNet.Data.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessNet.Desktop.Models.Events
{
    public class CasltingUpdateEvent
    {
        public PieceColor Color { get; set; }

        public CasltingUpdateEvent(PieceColor color)
        {
            Color = color;
        }
    }
}
