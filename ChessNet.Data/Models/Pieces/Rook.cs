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
            for (int i = Position.Row + 1; i < chessBoard.Rows; i++)
            {
                position = new BoardPosition(Position.Column, i);
                var (pieceMovement, isValid, isBreaking) = CheckPosition(chessBoard, position);
                if (isValid) yield return pieceMovement;
                if (isBreaking) break;
            }

            // + movement South
            for (int i = Position.Row - 1; i >= 0; i--)
            {
                position = new BoardPosition(Position.Column, i);
                var (pieceMovement, isValid, isBreaking) = CheckPosition(chessBoard, position);
                if (isValid) yield return pieceMovement;
                if (isBreaking) break;
            }

            // + movement East
            for (int i = Position.Column + 1; i < chessBoard.Columns; i++)
            {
                position = new BoardPosition(i, Position.Row);
                var (pieceMovement, isValid, isBreaking) = CheckPosition(chessBoard, position);
                if (isValid) yield return pieceMovement;
                if (isBreaking) break;
            }

            // + movement West
            for (int i = Position.Column - 1; i >= 0; i--)
            {
                position = new BoardPosition(i, Position.Row);
                var (pieceMovement, isValid, isBreaking) = CheckPosition(chessBoard, position);
                if (isValid) yield return pieceMovement;
                if (isBreaking) break;
            }
        }

        private (PieceMovement, bool, bool) CheckPosition(ChessBoard chessBoard, BoardPosition position)
        {
            if (chessBoard.IsValidPosition(position))
            {
                if (chessBoard.GetPiece(position) != null)
                    return (new PieceMovement(position, true), true, true);
                else
                    return (new PieceMovement(position, false), true, false);
            }
            else
                return (default, false, true);
        }
    }
}
