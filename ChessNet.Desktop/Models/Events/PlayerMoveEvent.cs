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
    public class PlayerMoveEvent
    {
        public MoveResult MoveResult { get; set; }
        public Player Player { get; set; }
        public GameStates State { get; set; }

        public PlayerMoveEvent(MoveResult moveResult, Player player, ChessGame game)
        {
            MoveResult = moveResult;
            Player = player;
            State = game.State;
        }
    }
}
