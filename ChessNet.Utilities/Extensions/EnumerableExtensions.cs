using ChessNet.Data.Models;
using ChessNet.Data.Structs;

namespace ChessNet.Utilities.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool IsEmpty<T>(this IEnumerable<T> current) where T : class
        {
            return current == null || !current.Any();
        }
    }
}