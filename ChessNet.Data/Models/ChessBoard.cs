using ChessNet.Data.Constants;
using ChessNet.Data.Structs;

namespace ChessNet.Data.Models
{
    public class ChessBoard
    {
        private Piece[,] _chessBoard { get; set; }

        public ChessBoard()
        {
            _chessBoard = new Piece[DefaultValues.BOARD_SIZE, DefaultValues.BOARD_SIZE];
        }

        public bool AddPiece(Piece piece)
        {
            if (IsValidPosition(piece.Position) &&
                GetPiece(piece.Position) == null)
            {
                _chessBoard[piece.Position.Column, piece.Position.Row] = piece;
            }

            return false;
        }

        public Piece GetPiece(int column, int row)
        {
            return _chessBoard[column, row];
        }

        public Piece GetPiece(BoardPosition boardPosition) =>
            GetPiece(boardPosition.Column, boardPosition.Row);

        public Piece GetPiece(Piece piece) =>
            GetPiece(piece.Position.Column, piece.Position.Row);

        public bool IsValidPosition(int column, int row)
        {
            if (column < 0) return false;
            if (row < 0) return false;
            if (column > _chessBoard.GetLength(0)) return false;
            if (row > _chessBoard.GetLength(1)) return false;

            return true;
        }

        public bool IsValidPosition(BoardPosition boardPosition) =>
            IsValidPosition(boardPosition.Column, boardPosition.Row);

        public bool IsValidPosition(Piece piece) =>
            IsValidPosition(piece.Position.Column, piece.Position.Row);

        public Piece MovePieceAndReturnCaptured(BoardPosition from, BoardPosition to)
        {
            Piece pieceAtDestination = _chessBoard[to.Column, to.Row];
            Piece originPiece = _chessBoard[from.Column, from.Row];
            
            _chessBoard[from.Column, from.Row] = null;
            _chessBoard[to.Column, to.Row] = originPiece;

            return pieceAtDestination;
        }
    }
}