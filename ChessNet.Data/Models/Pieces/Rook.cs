using ChessNet.Data.Enums;
using ChessNet.Data.Structs;

namespace ChessNet.Data.Models.Pieces
{
    public class Rook : Piece
    {
        public Rook(PieceColor pieceColor, BoardPosition boardPosition) 
            : base(pieceColor, PieceType.Rook, boardPosition)
        {

        }

        public override IEnumerable<PieceMovement> GetMovements(ChessBoard chessBoard)
        {
            BoardPosition position;

            // TODO: Castling
            // Castling may be done only if neither the king nor the rook has previously moved,
            // the squares between the king and the rook are unoccupied, the king is not in check,
            // and the king does not cross over or end up on a square attacked by an opposing piece.

            // + movement North
            for (int row = Position.Row + 1; row < chessBoard.Rows; row++)
            {
                position = new BoardPosition(Position.Column, row);
                var move = chessBoard.MoveTo(position);
                if (IsValidMove(move)) yield return move;

                // Cannot move past a piece on its movement path.
                if (move.IsValidPosition && move.PieceAtDestination != null) break;
            }

            // + movement South
            for (int row = Position.Row - 1; row >= 0; row--)
            {
                position = new BoardPosition(Position.Column, row);
                var move = chessBoard.MoveTo(position);
                if (IsValidMove(move)) yield return move;

                // Cannot move past a piece on its movement path.
                if (move.IsValidPosition && move.PieceAtDestination != null) break;
            }

            // + movement East
            for (int column = Position.Column + 1; column < chessBoard.Columns; column++)
            {
                position = new BoardPosition(column, Position.Row);
                var move = chessBoard.MoveTo(position);
                if (IsValidMove(move)) yield return move;

                // Cannot move past a piece on its movement path.
                if (move.IsValidPosition && move.PieceAtDestination != null) break;
            }

            // + movement West
            for (int column = Position.Column - 1; column >= 0; column--)
            {
                position = new BoardPosition(column, Position.Row);
                var move = chessBoard.MoveTo(position);
                if (IsValidMove(move)) yield return move;

                // Cannot move past a piece on its movement path.
                if (move.IsValidPosition && move.PieceAtDestination != null) break;
            }
        }

        private bool IsValidMove(PieceMovement move)
        {
            return move.IsValidPosition &&
                (move.PieceAtDestination is null || move.IsCaptureFor(Color));
        }
    }
}
