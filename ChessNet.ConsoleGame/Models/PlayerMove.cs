namespace ChessNet.ConsoleGame.Models
{
    internal class PlayerMove
    {
        public string From;
        public string To;

        public PlayerMove(string from, string to)
        {
            From = from;
            To = to;
        }
    }
}
