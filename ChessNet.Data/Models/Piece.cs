using ChessNet.Data.Constants;
using ChessNet.Data.Enums;
using ChessNet.Data.Structs;
using System.Diagnostics;

namespace ChessNet.Data.Models
{
    [DebuggerDisplay("{Color} {this.GetType().Name} at {Position} inPlay:{IsInChessBoard}")]
    public abstract class Piece
    {
        private BoardPosition _position { get; set; }

        public PieceColor Color { get; private set; }
        public bool IsFirstMove { get; private set; }
        public int Points { get; private set; }
        public ChessBoard ChessBoard { get; internal set; }

        public BoardPosition Position
        {
            get => _position;
            set
            {
                IsFirstMove = false;
                _position = value;
            }
        }

        public bool IsWhite => Color == PieceColor.White;

        public bool IsInChessBoard => ChessBoard != null;

        public Piece(PieceColor pieceColor, BoardPosition boardPosition, int points = PiecePoints.DEFAULT)
        {
            Color = pieceColor;
            Position = boardPosition;
            IsFirstMove = true;
            Points = points;
        }

        public abstract IEnumerable<PieceMovement> GetMovements();

        public abstract string GetSymbol();

        internal static IEnumerable<PieceMovement> CheckLineOfPositionsBasedOnPathStep(ChessBoard chessBoard, BoardPosition position, BoardPosition step, PieceColor color)
        {
            // Find start of step loop, given current position.
            BoardPosition checkingPosition = position;

            while (checkingPosition.Column >= 0 && checkingPosition.Row >= 0)
            {
                checkingPosition -= step;

                if (chessBoard.IsValidPosition(checkingPosition) &&
                    chessBoard.GetPiece(checkingPosition) != null)
                    break;
            }

            BoardPosition startingPosition = checkingPosition;

            // Step through each step, and avoid current piece placement, stopping when out of board.
            while (checkingPosition.Column < chessBoard.Columns && checkingPosition.Row < chessBoard.Rows)
            {
                if (checkingPosition == position)
                {
                    checkingPosition += step;
                    continue;
                }

                var move = chessBoard.MoveTo(checkingPosition);
                if (IsValidMove(move, color)) yield return move;

                // Cannot move past a piece on its movement path.
                if (move.IsValidPosition && 
                    move.PieceAtDestination != null &&
                    startingPosition != checkingPosition) break;

                checkingPosition += step;
            }
        }

        internal static bool TryGetValidPieceMovement(ChessBoard chessBoad, BoardPosition position, BoardPosition offset, PieceColor color, out PieceMovement pieceMovement)
        {
            var movePosition = position.GetOffset(offset);
            var move = chessBoad.MoveTo(movePosition);
            pieceMovement = default;

            if (IsValidMove(move, color))
            {
                pieceMovement = move;
                return true;
            }

            return false;
        }

        private static bool IsValidMove(PieceMovement move, PieceColor color)
        {
            return move.IsValidPosition &&
                (move.PieceAtDestination is null || move.IsCaptureFor(color));
        }
    }
}