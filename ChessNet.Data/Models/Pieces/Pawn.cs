using ChessNet.Data.Enums;
using ChessNet.Data.Structs;

namespace ChessNet.Data.Models.Pieces
{
    public class Pawn : Piece
    {
        public Pawn(PieceColor pieceColor, BoardPosition boardPosition) 
            : base(pieceColor, PieceType.Pawn, boardPosition)
        {

        }

        public override IEnumerable<PieceMovement> GetMovements(ChessBoard chessBoard)
        {
            BoardPosition position;
            bool isOcuppied;

            // Can move but not capture ahead
            if (IsFirstMove)
            {
                position = Position.GetOffset(0, (IsWhite ? 2 : -2));
                if (chessBoard.IsValidPosition(position))
                {
                    isOcuppied = chessBoard.GetPiece(position) != null;
                    if (!isOcuppied) yield return new PieceMovement(position, false);
                }
            }

            position = Position.GetOffset(0, (IsWhite ? 1 : -1));
            if (chessBoard.IsValidPosition(position))
            {
                isOcuppied = chessBoard.GetPiece(position) != null;
                if (!isOcuppied) yield return new PieceMovement(position, false);
            }

            // Can only capture on imediate diagonals
            position = Position.GetOffset(1, (IsWhite ? 1 : -1));
            if (chessBoard.IsValidPosition(position))
            {
                isOcuppied = chessBoard.GetPiece(position) != null;
                if (isOcuppied) yield return new PieceMovement(position, true);
            }

            position = Position.GetOffset(-1, (IsWhite ? 1 : -1));
            if (chessBoard.IsValidPosition(position))
            {
                isOcuppied = chessBoard.GetPiece(position) != null;
                if (isOcuppied) yield return new PieceMovement(position, true);
            }
        }
    }
}
