using ChessNet.Data.Constants;
using ChessNet.Data.Enums;
using ChessNet.Data.Extensions;
using ChessNet.Data.Structs;
using System.Text;

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
                piece.ChessBoard = this;
                return true;
            }

            return false;
        }

        public IEnumerable<Piece> GetPieces(PieceColor pieceColor)
        {
            return _chessBoard
                .Cast<Piece>()
                .Where(p => p?.Color == pieceColor);
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
            if (column >= _chessBoard.GetLength(0)) return false;
            if (row >= _chessBoard.GetLength(1)) return false;

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
            
            if (pieceAtDestination != null)
                pieceAtDestination.ChessBoard = null;

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

                foreach (Piece piece in _chessBoard)
                {
                    if (piece != null)
                        count++;
                }

                return count;
            }
        }

        public string PrintBoard()
        {
            StringBuilder sb = new();

            sb.Append($"  ");

            for (int c = 0; c < Columns; c++)
            {
                sb.Append($" {c.AsLetter()} ");
            }

            sb.Append($"\n");

            for (int r = 0; r < Rows; r++)
            {
                sb.Append($"{r + 1} ");

                for (int c = 0; c < Columns; c++)
                {
                    sb.Append($"[{(_chessBoard[c, r] != null ? (_chessBoard[c, r].Color == PieceColor.White ? "W" : "B") : " ")}]");
                }

                sb.Append("\n");
            }

            return sb.ToString();
        }
    }
}