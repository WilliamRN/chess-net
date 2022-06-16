using ChessNet.Data.Constants;
using ChessNet.Data.Enums;
using ChessNet.Data.Structs;

namespace ChessNet.Data.Models.Pieces
{
    public class King : Piece
    {
        private static readonly IEnumerable<BoardPosition> ValidMoveOffsets = new List<BoardPosition>()
        {
            new BoardPosition(-1, -1),
            new BoardPosition(-1, 0),
            new BoardPosition(-1, 1),

            new BoardPosition(0, 1),
            new BoardPosition(0, -1),

            new BoardPosition(1, 1),
            new BoardPosition(1, 0),
            new BoardPosition(1, -1),
        };

        public King(PieceColor pieceColor, BoardPosition boardPosition) 
            : base(pieceColor, boardPosition, PiecePoints.KING)
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
    }
}
