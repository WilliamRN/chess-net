using ChessNet.Data.Constants;
using ChessNet.Data.Enums;
using ChessNet.Data.Extensions;
using ChessNet.Data.Models.Pieces;
using ChessNet.Data.Structs;
using System.Text;

namespace ChessNet.Data.Models
{
    public class ChessBoard
    {
        public int Columns { get; private set; }
        public int Rows { get; private set; }

        private Piece[,] _chessBoard { get; set; }
        public PieceMovement LastMove { get; private set; }
        public object LastMovedPiece { get; private set; }

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

        public bool RemovePiece(Piece piece)
        {
            if (IsValidPosition(piece.Position))
            {
                var currentPiece = GetPiece(piece.Position);

                if (currentPiece != null)
                {
                    _chessBoard[currentPiece.Position.Column, currentPiece.Position.Row] = null;
                    piece.ChessBoard = currentPiece.ChessBoard = null;
                    return true;
                }
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

        public Piece MovePieceAndReturnCaptured<T>(T piece, PieceMovement pieceMovement) where T : Piece
        {
            if (pieceMovement.IsEnPassant && piece is Pawn)
                return MoveEnPassantAndReturnCaptured(piece as Pawn, pieceMovement);

            if (pieceMovement.IsCastling && piece is King)
                return MoveCastling(piece as King, pieceMovement);

            BoardPosition from = piece.Position;
            BoardPosition to = pieceMovement.Destination;

            if (piece is Pawn && Math.Abs(to.Row - from.Row) == 2)
            {
                var pieceAsPawn = piece as Pawn;
                pieceAsPawn.IsLastMoveTwoSpaces = true;
            }

            Piece destinationPiece = _chessBoard[to.Column, to.Row];
            Piece originPiece = _chessBoard[from.Column, from.Row];

            _chessBoard[from.Column, from.Row] = null;
            _chessBoard[to.Column, to.Row] = originPiece;
            piece.Position = originPiece.Position = to;

            if (destinationPiece != null)
                destinationPiece.ChessBoard = null;

            LastMove = pieceMovement;
            LastMovedPiece = piece;

            return destinationPiece;
        }

        public Piece MoveEnPassantAndReturnCaptured(Pawn piece, PieceMovement pieceMovement)
        {
            BoardPosition from = piece.Position;
            BoardPosition to = pieceMovement.Destination;
            BoardPosition capturedFrom = pieceMovement.PieceAtDestination.Position;

            Piece originPiece = _chessBoard[from.Column, from.Row];
            Piece capturedPiece = _chessBoard[capturedFrom.Column, capturedFrom.Row];

            _chessBoard[capturedFrom.Column, capturedFrom.Row] = null;
            _chessBoard[from.Column, from.Row] = null;
            _chessBoard[to.Column, to.Row] = originPiece;
            piece.Position = originPiece.Position = to;

            capturedPiece.ChessBoard = null;

            LastMove = pieceMovement;
            LastMovedPiece = piece;

            return capturedPiece;
        }

        public Piece MoveCastling(King king, PieceMovement pieceMovement)
        {
            Piece rook = pieceMovement.PieceAtDestination;
            int rookStep = (king.Position.Column - pieceMovement.Destination.Column) > 0 ? -1 : 1;

            BoardPosition rookPosition = new(king.Position.Column + rookStep, king.Position.Row);
            BoardPosition kingPosition = pieceMovement.Destination;

            _chessBoard[rook.Position.Column, rook.Position.Row] = null;
            _chessBoard[rookPosition.Column, rookPosition.Row] = rook;
            rook.Position = rookPosition;

            _chessBoard[king.Position.Column, king.Position.Row] = null;
            _chessBoard[kingPosition.Column, kingPosition.Row] = king;
            king.Position = kingPosition;

            return null;
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

        public IEnumerable<Piece> AttackersFor(PieceColor color, BoardPosition position)
        {
            IEnumerable<Piece> result = new List<Piece>();
            
            // TODO: ignore self piece on checking for attackers.

            return result
                .Concat(King.GetKingAttackersFor(this, color, position))
                .Concat(Pawn.GetPawnAttackersFor(this, color, position))
                .Concat(Knight.GetKnightAttackersFor(this, color, position))
                .Concat(Bishop.GetBishopAttackersFor(this, color, position))
                .Concat(Rook.GetRookAttackersFor(this, color, position))
                .Concat(Queen.GetQueenAttackersFor(this, color, position));
        }

        public IEnumerable<Piece> AttackersFor(Piece piece) => AttackersFor(piece.Color, piece.Position);

        public string PrintBoard()
        {
            StringBuilder sb = new();

            sb.Append($"  ");

            for (int c = 0; c < Columns; c++)
            {
                sb.Append($" {c.ToColumnAnnotation()}♙");
            }

            sb.Append($"\n");

            for (int r = Rows - 1; r >= 0; r--)
            {
                sb.Append($"{r + 1} ");

                for (int c = 0; c < Columns; c++)
                {
                    sb.Append($"[{(_chessBoard[c, r] != null ? _chessBoard[c, r].GetSymbol() : "﹘")}]");
                }

                sb.Append("\n");
            }

            return sb.ToString();
        }
    }
}