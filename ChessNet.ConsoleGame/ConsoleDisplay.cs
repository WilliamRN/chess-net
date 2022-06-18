using ChessNet.ConsoleGame.Constants;
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

        public string GetPlayerName(bool isWhite = true)
        {
            StringBuilder sb = new StringBuilder();
            string PlayerName;

            sb.AppendLine("");
            sb.AppendLine($"Enter the {(isWhite ? "first" : "second")} player's name ({(isWhite ? "white" : "black")} pieces):");

            Console.Write(sb.ToString());

            PlayerName = Console.ReadLine() ?? "";
            PlayerName = string.IsNullOrWhiteSpace(PlayerName)
                ? (isWhite ? DefaultValues.PLAYER_1 : DefaultValues.PLAYER_2)
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
            sb.AppendLine($"Game state: {gameManager.State}");
            sb.AppendLine(gameManager.GetScore());
            sb.AppendLine("Current board:");
            sb.AppendLine("");
            Console.Write(sb.ToString());

            gameManager.PrintBoardToConcole();

            sb.Clear();
            sb.AppendLine("-----------------------");
            Console.Write(sb.ToString());
        }

        public bool GetNextMove(GameManager gameManager)
        {
            StringBuilder sb = new StringBuilder();
            string from, to;

            sb.AppendLine("");
            sb.AppendLine($"It's {gameManager.GetPlayerName()}'s {gameManager.GetPlayerColor()} pieces turn!");
            sb.AppendLine("Next move from:");
            Console.Write(sb.ToString());

            from = Console.ReadLine() ?? "";

            sb.Clear();
            sb.AppendLine($"Moving from {from} to:");
            Console.Write(sb.ToString());

            to = Console.ReadLine() ?? "";

            if (DefaultValues.ACTION_DELAY > 100)
            {
                sb.Clear();
                sb.AppendLine($"Moving from {from} to {to}!");
                Console.Write(sb.ToString());

                Thread.Sleep(DefaultValues.ACTION_DELAY);
            }

            return gameManager.MakeMove(from, to);
        }

        public void MainGameLoop()
        {
            GameManager gameManager = new(GetPlayerName(), GetPlayerName(false));
            PrintSettingUpBoard();

            while (gameManager.State != "End" && gameManager.State != "CheckMate")
            {
                ClearScreen();
                PrintGameHeader(gameManager);
                GetNextMove(gameManager);
            }

            PrintGameEnd(gameManager);
        }
    }
}
