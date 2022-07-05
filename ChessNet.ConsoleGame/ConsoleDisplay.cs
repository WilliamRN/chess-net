using ChessNet.ConsoleGame.Constants;
using ChessNet.ConsoleGame.Enums;
using ChessNet.Data.Enums;
using System.Text;

namespace ChessNet.ConsoleGame
{
    internal class ConsoleDisplay
    {
        public void ClearScreen()
        {
            Console.Clear();
        }

        public void PrintWelcomeMessage()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("");
            sb.AppendLine("Welcome to ChessNet!");

            Console.Write(sb.ToString());
        }

        public void PrintGameEnd(GameManager gameManager)
        {
            ClearScreen();
            PrintGameHeader(gameManager);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("");
            sb.AppendLine("Finish!");
            sb.AppendLine($"Too bad {gameManager.GetPlayerName()}, you lose!");

            Console.Write(sb.ToString());

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        public string GetPlayerName(PieceColor color = PieceColor.White)
        {
            StringBuilder sb = new StringBuilder();
            string PlayerName;

            sb.AppendLine("");
            sb.AppendLine($"Enter the {(color == PieceColor.White ? "first" : "second")} player's name ({color.ToString()} pieces):");
            Console.Write(sb.ToString());

            PlayerName = Console.ReadLine() ?? "";
            PlayerName = string.IsNullOrWhiteSpace(PlayerName)
                ? (color == PieceColor.White ? DefaultValues.PLAYER_1 : DefaultValues.PLAYER_2)
                : PlayerName;

            return PlayerName;
        }

        public void PrintSettingUpBoard()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("");
            sb.AppendLine("Ok!");
            sb.AppendLine("Setting the board...");
            Console.Write(sb.ToString());

            Thread.Sleep(DefaultValues.ACTION_DELAY);
        }

        public void PrintGameHeader(GameManager gameManager)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(" - ChessNet - ");
            sb.AppendLine("");
            sb.AppendLine($"Last move: {gameManager.LastFrom} -> {gameManager.LastTo}, {(gameManager.IsLastMoveValid ? "Valid move!" : "Move was not valid...")}");
            sb.AppendLine($"Message: {gameManager.Message}");
            sb.AppendLine($"Game state: {gameManager.State.ToString()}");
            sb.AppendLine(gameManager.GetScore());
            sb.AppendLine("Current board:");
            sb.AppendLine("");
            Console.Write(sb.ToString());

            gameManager.PrintBoardToConcole();

            sb.Clear();
            sb.AppendLine("-----------------------");
            Console.Write(sb.ToString());
        }

        public GameplayMode GetGameplayMode()
        {
            StringBuilder sb = new StringBuilder();

            GameplayMode defaultMode = DefaultValues.GAMEPLAY_MODE;

            sb.AppendLine("");
            sb.AppendLine($"Choose gameplay mode (default: {defaultMode}):");
            sb.AppendLine($"  {(int)GameplayMode.TwoPlayers}. {GameplayMode.TwoPlayers.ToString()}");
            sb.AppendLine($"  {(int)GameplayMode.OnePlayer}. {GameplayMode.OnePlayer.ToString()}");
            sb.AppendLine($"  {(int)GameplayMode.AIOnly}. {GameplayMode.AIOnly.ToString()}");
            Console.Write(sb.ToString());

            var input = Console.ReadLine() ?? $"{defaultMode}";

            var selected = Enum.TryParse(input, out GameplayMode result) 
                ? result : defaultMode;

            if (!Enum.IsDefined(typeof(GameplayMode), selected))
                selected = defaultMode;

            Console.WriteLine($"mode selected: {selected}");

            return selected;
        }
    }
}
