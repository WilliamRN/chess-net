using ChessNet.Data.Constants;
using ChessNet.Data.Enums;
using ChessNet.Data.Structs;

namespace ChessNet.Data.Models
{
    public abstract class Piece
    {
        public PieceColor Color { get; private set; }
        public bool IsFirstMove { get; private set; }
        public int Points { get; private set; }

        public ChessBoard ChessBoard { get; internal set; }
        private BoardPosition _position { get; set; }
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

        internal IEnumerable<PieceMovement> CheckLineOfPositionsBasedOnPathStep(BoardPosition step)
        {
            // Find start of step loop, given current position.
            BoardPosition checkingPosition = Position;

            while (checkingPosition.Column >= 0 && checkingPosition.Row >= 0)
            {
                checkingPosition -= step;

                if (ChessBoard.IsValidPosition(checkingPosition) &&
                    ChessBoard.GetPiece(checkingPosition) != null)
                    break;
            }

            BoardPosition startingPosition = checkingPosition;

            // Step through each step, and avoid current piece placement, stopping when out of board.
            while (checkingPosition.Column < ChessBoard.Columns && checkingPosition.Row < ChessBoard.Rows)
            {
                if (checkingPosition == Position)
                {
                    checkingPosition += step;
                    continue;
                }

                var move = ChessBoard.MoveTo(checkingPosition);
                if (IsValidMove(move)) yield return move;

                // Cannot move past a piece on its movement path.
                if (move.IsValidPosition && 
                    move.PieceAtDestination != null &&
                    startingPosition != checkingPosition) break;

                checkingPosition += step;
            }
        }

        internal bool TryGetValidPieceMovement(BoardPosition offset, out PieceMovement pieceMovement)
        {
            var position = Position.GetOffset(offset);
            var move = ChessBoard.MoveTo(position);
            pieceMovement = default;

            if (IsValidMove(move))
            {
                pieceMovement = move;
                return true;
            }

            return false;
        }

        private bool IsValidMove(PieceMovement move)
        {
            return move.IsValidPosition &&
                (move.PieceAtDestination is null || move.IsCaptureFor(Color));
        }
    }
}