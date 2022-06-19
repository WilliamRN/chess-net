using ChessNet.Data.Structs;

namespace ChessNet.AI.Interfaces
{
    public interface IPlayerAI
    {
        public PieceMovement GetNextMove();
    }
}