using ChessNet.ConsoleGame.Constants;
using ChessNet.Data.Extensions;
using ChessNet.Data.Models;
using ChessNet.Data.Structs;

namespace ChessNet.ConsoleGame
{
    internal class GameManager
    {
        private ChessGame _game;
        private string _whitePlayerName;
        private string _blackPlayerName;
        
        public string Message;
        public string State;
        public string LastFrom;
        public string LastTo;
        public bool IsLastMoveValid;

        public GameManager(string whitePlayerName, string blackPlayerName)
        {
            _game = new();
            _whitePlayerName = whitePlayerName;
            _blackPlayerName = blackPlayerName;
            Message = string.Format(MessageFormats.GAME_STARTED, GetPlayerName());
            State = _game.State.ToString();
            IsLastMoveValid = false;
        }

        public string GetPlayerName()
        {
            return _game.CurrentPlayer.Color == Data.Enums.PieceColor.White
                ? _whitePlayerName : _blackPlayerName;
        }

        public string GetPlayerColor()
        {
            return _game.CurrentPlayer.Color == Data.Enums.PieceColor.White
                ? "white" : "black";
        }

        public string GetScore()
        {
            string score = "";

            score += $"{_whitePlayerName}'s (white) score: {_game.WhiteScore}\n";
            score += $"{_blackPlayerName}'s (black) score: {_game.BlackScore}\n";

            return score;
        }

        public void PrintBoardToConcole() => _game.Board.PrintToConsole();

        public bool MakeMove(string origin, string destiny)
        {
            BoardPosition from = new(origin);
            BoardPosition to = new(destiny);
            bool result;

            LastFrom = from.AsString();
            LastTo = to.AsString();

            try
            {
                if (_game.Move(from, to))
                {
                    Message = string.Format(MessageFormats.MOVED_PIECE, from.AsString(), to.AsString());
                    Message += " " + string.Format(MessageFormats.NEW_TURN, GetPlayerName());
                    result = true;
                }
                else
                {
                    Message += " " + string.Format(MessageFormats.COULD_NOT_MOVE, from.AsString(), to.AsString(), "Move is not valid!");
                    result = false;
                }
            }
            catch (Exception ex)
            {
                Message += " " + string.Format(MessageFormats.COULD_NOT_MOVE, from.AsString(), to.AsString(), ex.Message);
                result = false;
            }

            State = _game.State.ToString();
            IsLastMoveValid = result;
            return result;
        }
    }
}
