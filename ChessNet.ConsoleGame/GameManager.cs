using ChessNet.ConsoleGame.Constants;
using ChessNet.Data.Extensions;
using ChessNet.Data.Models;
using ChessNet.Data.Structs;

namespace ChessNet.ConsoleGame
{
    internal class GameManager
    {
        private string _whitePlayerName;
        private string _blackPlayerName;
        
        public ChessGame ChessGame { get; private set; }
        public string Message { get; private set; }
        public string State { get; private set; }
        public string LastFrom { get; private set; }
        public string LastTo { get; private set; }
        public bool IsLastMoveValid { get; private set; }

        public GameManager(string whitePlayerName, string blackPlayerName)
        {
            ChessGame = new();
            _whitePlayerName = whitePlayerName;
            _blackPlayerName = blackPlayerName;
            Message = string.Format(MessageFormats.GAME_STARTED, GetPlayerName());
            State = ChessGame.State.ToString();
            IsLastMoveValid = false;
            LastFrom = "";
            LastTo = "";
        }

        public string GetPlayerName()
        {
            return ChessGame.CurrentPlayer.Color == Data.Enums.PieceColor.White
                ? _whitePlayerName : _blackPlayerName;
        }

        public string GetPlayerColor()
        {
            return ChessGame.CurrentPlayer.Color == Data.Enums.PieceColor.White
                ? "white" : "black";
        }

        public string GetScore()
        {
            string score = "";

            score += $"{_whitePlayerName}'s (white) score: {ChessGame.WhiteScore}\n";
            score += $"{_blackPlayerName}'s (black) score: {ChessGame.BlackScore}\n";

            return score;
        }

        public void PrintBoardToConcole() => ChessGame.Board.PrintToConsole();

        public bool MakeMove(BoardPosition from, BoardPosition to)
        {
            bool result;

            try
            {
                LastFrom = from.AsString();
                LastTo = to.AsString();

                if (ChessGame.Move(from, to))
                {
                    Message = string.Format(MessageFormats.MOVED_PIECE, from.AsString(), to.AsString());
                    Message += " " + string.Format(MessageFormats.NEW_TURN, GetPlayerName());
                    result = true;
                }
                else
                {
                    Message = string.Format(MessageFormats.COULD_NOT_MOVE, from.AsString(), to.AsString(), "Move is not valid!");
                    result = false;
                }
            }
            catch (Exception ex)
            {
                Message = string.Format(MessageFormats.COULD_NOT_MOVE, from.AsString(), to.AsString(), ex.Message);
                result = false;
            }

            State = ChessGame.State.ToString();
            IsLastMoveValid = result;
            return result;
        }

        public bool MakeMove(string origin, string destiny)
        {
            bool result;
            BoardPosition from = new();
            BoardPosition to = new();

            try
            {
                from = new(origin);
                to = new(destiny);

                return MakeMove(from, to);
            }
            catch (Exception ex)
            {
                Message = string.Format(MessageFormats.COULD_NOT_MOVE, from.AsString(), to.AsString(), ex.Message);
                result = false;
            }

            State = ChessGame.State.ToString();
            IsLastMoveValid = result;
            return result;
        }
    }
}
