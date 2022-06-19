using ChessNet.Data.Constants;
using ChessNet.Data.Enums;
using ChessNet.Data.Models.Pieces;
using ChessNet.Data.Structs;
using System.Diagnostics;

namespace ChessNet.Data.Models
{
    [DebuggerDisplay("{Color} {this.GetType().Name} at {Position} inPlay:{IsInChessBoard}")]
    public abstract class Piece
    {
        private BoardPosition _position { get; set; }
        private Func<GameStates> _stateGetter { get; set; }

        public PieceColor Color { get; private set; }
        public bool IsFirstMove { get; private set; }
        public int Points { get; private set; }
        public ChessBoard Board { get; internal set; }

        public BoardPosition Position
        {
            get => _position;
            set
            {
                IsFirstMove = false;
                _position = value;
            }
        }

        internal GameStates State
        {
            get
            {
                var result = GameStates.InvalidGameState;

                try
                {
                    if (_stateGetter != null)
                        return _stateGetter();
                }
                catch
                {
                    result = GameStates.Error;
                }

                return result;
            }
        }

        public bool IsWhite => Color == PieceColor.White;

        public bool IsInChessBoard => Board != null;

        public Piece(PieceColor pieceColor, BoardPosition boardPosition, int points = PiecePoints.DEFAULT)
        {
            Color = pieceColor;
            Position = boardPosition;
            IsFirstMove = true;
            Points = points;
        }

        public abstract IEnumerable<Movement> GetMovements();

        public abstract string GetSymbol();

        internal void SetStateGetter(Func<GameStates> stateGetter) => _stateGetter = stateGetter;

        internal static IEnumerable<Movement> CheckLineOfPositionsBasedOnPathStep(ChessBoard chessBoard, BoardPosition position, BoardPosition step, PieceColor color)
        {
            // Find start of step loop, given current position.
            BoardPosition checkingPosition = position;

            while (checkingPosition.Column >= 0 && checkingPosition.Row >= 0)
            {
                if (!chessBoard.IsValidPosition(checkingPosition - step))
                    break;

                checkingPosition -= step;

                if (chessBoard.GetPiece(checkingPosition) != null)
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

        internal static bool TryGetValidPieceMovement(ChessBoard chessBoad, BoardPosition position, BoardPosition offset, PieceColor color, out Movement pieceMovement)
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

        private static bool IsValidMove(Movement move, PieceColor color)
        {
            return move.IsValidPosition &&
                (move.PieceAtDestination is null || move.IsCaptureFor(color));
        }

        internal bool IsValidMoveForCurrentKingPosition<T>(T piece, Movement move) where T : Piece
        {
            // King should be under attack at this state.
            if (State == GameStates.Check)
            {
                var king = Board
                    .GetPieces(Color)
                    .FirstOrDefault(p => p is King);

                var previewBoard = Board.GetPreviewBoardFor(piece, move.Destination);

                if (!previewBoard.AttackersFor(king).Any())
                    return true;
                else
                    return false;
            }

            return true;
        }
    }
}