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
    public class PlayerMoveResultEvent
    {
        public MoveResult MoveResult { get; set; }
        public Player Player { get; set; }
        public Player CurrentPlayer { get; set; }
        public GameStates State { get; set; }
        public int WhiteScore { get; set; }
        public int BlackScore { get; set; }

        public PlayerMoveResultEvent(MoveResult moveResult, Player player, ChessGame game)
        {
            MoveResult = moveResult;
            Player = player;
            CurrentPlayer = game.CurrentPlayer;
            State = game.State;
            WhiteScore = game.WhiteScore;
            BlackScore = game.BlackScore;
        }
    }
}
