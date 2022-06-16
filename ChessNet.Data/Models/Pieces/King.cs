using ChessNet.Data.Enums;
using ChessNet.Data.Structs;

namespace ChessNet.Data.Models.Pieces
{
    public class King : Piece
    {
        private static readonly IEnumerable<BoardPosition> ValidMoveOffsets = new List<BoardPosition>()
        {
            new BoardPosition(0, 1),
            new BoardPosition(0, -1),
            new BoardPosition(1, 0),
            new BoardPosition(-1, 0),
        };

        public King(PieceColor pieceColor, BoardPosition boardPosition) 
            : base(pieceColor, PieceType.King, boardPosition)
        {

        }

        public override IEnumerable<PieceMovement> GetMovements(ChessBoard chessBoard)
        {
            // TODO: Castling
            // Castling may be done only if neither the king nor the rook has previously moved,
            // the squares between the king and the rook are unoccupied, the king is not in check,
            // and the king does not cross over or end up on a square attacked by an opposing piece.

            foreach (var validOffset in ValidMoveOffsets)
            {
                if (TryGetValidPieceMovement(validOffset, chessBoard, out PieceMovement value))
                    yield return value;
            }
        }

        private bool TryGetValidPieceMovement(BoardPosition offset, ChessBoard chessBoard, out PieceMovement pieceMovement)
        {
            var position = Position.GetOffset(offset);
            var move = chessBoard.MoveTo(position);
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
