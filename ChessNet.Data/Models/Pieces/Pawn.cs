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
            PieceMovement move;
            var isPieceAhead = false;

            // TODO: Promotion to Queen, at board edge.

            position = Position.GetOffset(0, IsWhite ? 1 : -1);
            move = chessBoard.MoveTo(position);

            if (move.IsValidPosition &&
                move.PieceAtDestination is null)
                yield return move;
            else
                isPieceAhead = true;

            // Can move but not capture ahead
            if (IsFirstMove && !isPieceAhead)
            {
                position = Position.GetOffset(0, IsWhite ? 2 : -2);
                move = chessBoard.MoveTo(position);

                if(move.IsValidPosition &&
                    move.PieceAtDestination is null)
                    yield return move;
            }

            // Can only capture on imediate diagonals
            position = Position.GetOffset(1, IsWhite ? 1 : -1);
            move = chessBoard.MoveTo(position);

            if (move.IsValidPosition &&
                move.PieceAtDestination != null && 
                move.PieceAtDestination.Color != Color)
                yield return move;

            position = Position.GetOffset(-1, IsWhite ? 1 : -1);
            move = chessBoard.MoveTo(position);

            if (move.IsValidPosition &&
                move.PieceAtDestination != null && 
                move.PieceAtDestination.Color != Color)
                yield return move;
        }
    }
}
