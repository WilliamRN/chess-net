using ChessNet.Data.Enums;
using ChessNet.Data.Structs;

namespace ChessNet.Data.Models
{
    public class MoveResult
    {
        public PieceColor Player { get; set; }
        public GameStates State { get; set; }
        public BoardPosition From { get; set; }
        public BoardPosition To { get; set; }
        public bool IsValid { get; set; }
        public Piece CapturedPiece { get; set; }

        public bool IsCapture => CapturedPiece != null;
        public bool IsSurrender => State == GameStates.Surrender;

        public MoveResult(PieceColor playerColor, GameStates state, BoardPosition from, BoardPosition to, bool isValid)
        {
            Player = playerColor;
            State = state;
            From = from;
            To = to;
            IsValid = isValid;
        }

        public MoveResult(PieceColor playerColor, GameStates state, BoardPosition from, BoardPosition to, bool isValid, Piece capturedPiece)
            : this(playerColor, state, from, to, isValid)
        {
            CapturedPiece = capturedPiece;
        }
    }
}
