using ChessNet.Data.Enums;
using ChessNet.Data.Structs;

namespace ChessNet.Data.Constants
{
    public static class BoardDirectionSteps
    {
        public static BoardPosition HORIZONTAL_STEP = new(1, 0);
        public static BoardPosition VERTICAL_STEP = new(0, 1);
        public static BoardPosition DIAGONAL_STEP = new(1, 1);
    }
}