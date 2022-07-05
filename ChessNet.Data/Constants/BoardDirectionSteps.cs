using ChessNet.Data.Enums;
using ChessNet.Data.Structs;

namespace ChessNet.Data.Constants
{
    public static class BoardDirectionSteps
    {
        public static readonly BoardPosition HORIZONTAL_STEP = new(1, 0);
        public static readonly BoardPosition VERTICAL_STEP = new(0, 1);
        public static readonly BoardPosition DIAGONAL_STEP_FOWARD = new(1, 1);
        public static readonly BoardPosition DIAGONAL_STEP_BACK = new(1, -1);
    }
}