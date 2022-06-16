using ChessNet.Data.Constants;
using ChessNet.Data.Structs;
using System.Diagnostics;

namespace ChessNet.Data.Models
{
    public class ChessBoard
    {
        public int Columns { get; private set; }
        public int Rows { get; private set; }

        private Piece[,] _chessBoard { get; set; }

        public ChessBoard()
        {
            Columns = DefaultValues.BOARD_SIZE;
            Rows = DefaultValues.BOARD_SIZE;
            _chessBoard = new Piece[Columns, Rows];
        }

        public bool AddPiece(Piece piece)
        {
            if (IsValidPosition(piece.Position) &&
                GetPiece(piece.Position) == null)
            {
                _chessBoard[piece.Position.Column, piece.Position.Row] = piece;
                return true;
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
            originPiece.Position = to;

            return pieceAtDestination;
        }

        public PieceMovement MoveTo(BoardPosition position)
        {
            if (IsValidPosition(position))
            {
                var piece = GetPiece(position);
                return new PieceMovement(position, piece);
            }

            return default;
        }

        public int PieceCount
        {
            get
            {
                var count = 0;

                foreach(Piece piece in _chessBoard)
                {
                    if (piece != null)
                        count++;
                }

                return count;
            }
        }
    }
}