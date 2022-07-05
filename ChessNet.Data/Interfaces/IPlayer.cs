using ChessNet.Data.Enums;
using ChessNet.Data.Structs;

namespace ChessNet.Data.Interfaces
{
    public interface IPlayer
    {
        public PieceMovement GetNextMove();
        public string GetName();
        public PieceColor GetColor();
    }
}