using ChessNet.ConsoleGame.Constants;
using ChessNet.Data.Enums;
using ChessNet.Data.Interfaces;
using ChessNet.Data.Structs;
using System.Text;

namespace ChessNet.ConsoleGame.Players
{
    internal class HumanConsolePlayer : IPlayer
    {
        private string _name;
        private PieceColor _color;

        public HumanConsolePlayer(string name, PieceColor color)
        {
            _name = name;
            _color = color;
        }

        public PieceColor GetColor() => _color;

        public string GetName() => _name;

        public PieceMovement GetNextMove()
        {
            StringBuilder sb = new StringBuilder();
            string from, to;

            sb.AppendLine("");
            sb.AppendLine($"It's {_name}'s {_color.ToString().ToLower()} pieces turn!");
            sb.AppendLine("Next move from:");
            Console.Write(sb.ToString());

            from = Console.ReadLine() ?? "";

            sb.Clear();
            sb.AppendLine($"Moving from {from} to:");
            Console.Write(sb.ToString());

            to = Console.ReadLine() ?? "";

            Thread.Sleep(DefaultValues.ACTION_DELAY);

            BoardPosition fromPosition = new(from);
            BoardPosition toPosition = new(to);

            return new PieceMovement(fromPosition, toPosition);
        }
    }
}
