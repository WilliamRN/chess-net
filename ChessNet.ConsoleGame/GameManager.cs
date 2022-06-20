using ChessNet.AI.RamdomInputsAI;
using ChessNet.ConsoleGame.Constants;
using ChessNet.ConsoleGame.Enums;
using ChessNet.ConsoleGame.Players;
using ChessNet.Data.Enums;
using ChessNet.Data.Extensions;
using ChessNet.Data.Interfaces;
using ChessNet.Data.Models;
using ChessNet.Data.Structs;

namespace ChessNet.ConsoleGame
{
    internal class GameManager
    {
        private IPlayer _playerWhite;
        private IPlayer _playerBlack;

        private string _whitePlayerName;
        private string _blackPlayerName;
        private ConsoleDisplay _consoleDisplay;
        private int _actionDelay;
        
        public ChessGame ChessGame { get; private set; }
        public string Message { get; private set; }
        public string LastFrom { get; private set; }
        public string LastTo { get; private set; }
        public bool IsLastMoveValid { get; private set; }

        public GameStates State => ChessGame.State;

        public GameManager(ConsoleDisplay consoleDisplay)
        {
            ChessGame = new();
            _whitePlayerName = DefaultValues.PLAYER_1;
            _blackPlayerName = DefaultValues.PLAYER_2;
            _consoleDisplay = consoleDisplay;
            _actionDelay = DefaultValues.ACTION_DELAY;
            Message = string.Format(MessageFormats.GAME_STARTED, GetPlayerName());
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

        public bool MakeMove(PieceMovement move)
        {
            bool result;

            try
            {
                LastFrom = move.FromPosition.AsString();
                LastTo = move.ToPosition.AsString();

                if (ChessGame.Move(move))
                {
                    Message = string.Format(MessageFormats.MOVED_PIECE, move.FromPosition.AsString(), move.ToPosition.AsString());
                    Message += " " + string.Format(MessageFormats.NEW_TURN, GetPlayerName());
                    result = true;
                }
                else
                {
                    Message = string.Format(MessageFormats.COULD_NOT_MOVE, move.FromPosition.AsString(), move.ToPosition.AsString(), "Move is not valid!");
                    result = false;
                }
            }
            catch (Exception ex)
            {
                Message = string.Format(MessageFormats.COULD_NOT_MOVE, move.FromPosition.AsString(), move.ToPosition.AsString(), ex.Message);
                result = false;
            }

            IsLastMoveValid = result;
            return result;
        }

        public void Configure()
        {
            GameplayMode mode = _consoleDisplay.GetGameplayMode();

            switch (mode)
            {
                case GameplayMode.TwoPlayers:

                    _whitePlayerName = _consoleDisplay.GetPlayerName(PieceColor.White);
                    _playerWhite = new HumanConsolePlayer(_whitePlayerName, PieceColor.White);

                    _blackPlayerName = _consoleDisplay.GetPlayerName(PieceColor.Black);
                    _playerBlack = new HumanConsolePlayer(_blackPlayerName, PieceColor.Black);
                    break;

                case GameplayMode.AIOnly:

                    _playerWhite = new RamdomAI(ChessGame, PieceColor.White, "Computer One");
                    _whitePlayerName = _playerWhite.GetName();

                    _playerBlack = new RamdomAI(ChessGame, PieceColor.Black, "Computer Two");
                    _blackPlayerName = _playerBlack.GetName();

                    _actionDelay = DefaultValues.ACTION_DELAY_AI_ONLY;
                    break;

                default:
                case GameplayMode.OnePlayer:

                    _whitePlayerName = _consoleDisplay.GetPlayerName(PieceColor.White);
                    _playerWhite = new HumanConsolePlayer(_whitePlayerName, PieceColor.White);

                    _playerBlack = new RamdomAI(ChessGame, PieceColor.Black);
                    _blackPlayerName = _playerBlack.GetName();
                    break;
            }
        }

        public void StartMainGameLoop()
        {
            PieceMovement move = default;
            _consoleDisplay.PrintSettingUpBoard();

            while (!ChessGame.IsFinished)
            {
                _consoleDisplay.ClearScreen();
                _consoleDisplay.PrintGameHeader(this);

                try
                {
                    move = default;

                    if (ChessGame.CurrentPlayer.Color == PieceColor.White)
                        move = _playerWhite.GetNextMove();
                    else
                        move = _playerBlack.GetNextMove();

                    MakeMove(move);

                    Thread.Sleep(_actionDelay);
                }
                catch (Exception ex)
                {
                    Message = string.Format(MessageFormats.COULD_NOT_MOVE, move.FromPosition.AsString(), move.ToPosition.AsString(), ex.Message);
                    IsLastMoveValid = false;
                }
            }

            _consoleDisplay.PrintGameEnd(this);
        }
    }
}
