using ChessNet.Data.Models;
using ChessNet.Data.Models.Pieces;
using System.Text;

namespace ChessNet.Data.Extensions
{
    public static class ChessBoardConsoleExtensions
    {
        public static void PrintToConsole(this ChessBoard chessBoard)
        {
            Piece[,] board = chessBoard.GetBoard();

            // Header
            Console.Write("  ");

            for (int c = 0; c < chessBoard.Columns; c++)
                Console.Write($" {c.ToColumnAnnotation()} ");

            Console.Write("\n");

            // Rows
            for (int r = chessBoard.Rows - 1; r >= 0; r--)
            {
                Console.Write($"{r + 1} ");

                for (int c = 0; c < chessBoard.Columns; c++)
                {
                    Console.Write($"[");

                    board[c, r].GetConsoleColor().SetColor();
                    Console.Write($"{board[c, r].GetLetter()}");
                    ConsoleBoardColor.DefaultColor.SetColor();

                    Console.Write($"]");
                }

                Console.Write("\n");
            }

            Console.Write("\n");
        }


        private static ConsoleBoardColor GetConsoleColor(this Piece piece)
        {
            if (piece == null)
                return ConsoleBoardColor.EmptySpace;
            else if (piece.Color == Enums.PieceColor.White)
                return ConsoleBoardColor.White;
            else
                return ConsoleBoardColor.Black;
        }

        private static void SetColor(this ConsoleBoardColor color)
        {
            switch (color)
            {
                case ConsoleBoardColor.EmptySpace:
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;

                case ConsoleBoardColor.White:
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.White;
                    break;

                case ConsoleBoardColor.Black:
                    Console.BackgroundColor = ConsoleColor.Magenta;
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;

                case ConsoleBoardColor.DefaultColor:
                default:
                    Console.ResetColor();
                    break;
            }
        }

        private static string GetLetter(this Piece piece)
        {
            if (piece == null) return " ";
            if (piece is Pawn) return "P";
            if (piece is King) return "K";
            if (piece is Rook) return "R";
            if (piece is Queen) return "Q";
            if (piece is Bishop) return "B";
            if (piece is Knight) return "N";

            return "?";
        }

        private enum ConsoleBoardColor
        {
            DefaultColor,
            EmptySpace,
            White,
            Black,
        }
    }
}
