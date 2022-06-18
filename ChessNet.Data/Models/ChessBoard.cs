using ChessNet.Data.Constants;
using ChessNet.Data.Enums;
using ChessNet.Data.Extensions;
using ChessNet.Data.Models.Pieces;
using ChessNet.Data.Structs;
using System.Text;

namespace ChessNet.Data.Models
{
    public class ChessBoard : ICloneable
    {
        public int Columns { get; private set; }
        public int Rows { get; private set; }

        private Piece[,] _board { get; set; }
        public PieceMovement LastMove { get; private set; }
        public object LastMovedPiece { get; private set; }

        public ChessBoard(int columns = DefaultValues.BOARD_SIZE, int rows = DefaultValues.BOARD_SIZE)
        {
            Columns = columns;
            Rows = rows;
            _board = new Piece[Columns, Rows];
        }

        public bool AddPiece(Piece piece)
        {
            if (IsValidPosition(piece.Position) &&
                GetPiece(piece.Position) == null)
            {
                _board[piece.Position.Column, piece.Position.Row] = piece;
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
                    _board[currentPiece.Position.Column, currentPiece.Position.Row] = null;
                    piece.ChessBoard = currentPiece.ChessBoard = null;
                    return true;
                }
            }

            return false;
        }

        public IEnumerable<Piece> GetPieces(PieceColor pieceColor)
        {
            return _board
                .Cast<Piece>()
                .Where(p => p?.Color == pieceColor);
        }

        public Piece GetPiece(int column, int row)
        {
            return _board[column, row];
        }

        public Piece GetPiece(BoardPosition boardPosition) =>
            GetPiece(boardPosition.Column, boardPosition.Row);

        public Piece GetPiece(Piece piece) =>
            GetPiece(piece.Position.Column, piece.Position.Row);

        public bool IsValidPosition(int column, int row)
        {
            if (column < 0) return false;
            if (row < 0) return false;
            if (column >= _board.GetLength(0)) return false;
            if (row >= _board.GetLength(1)) return false;

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

            Piece destinationPiece = _board[to.Column, to.Row];
            Piece originPiece = _board[from.Column, from.Row];

            _board[from.Column, from.Row] = null;
            _board[to.Column, to.Row] = originPiece;
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

            Piece originPiece = _board[from.Column, from.Row];
            Piece capturedPiece = _board[capturedFrom.Column, capturedFrom.Row];

            _board[capturedFrom.Column, capturedFrom.Row] = null;
            _board[from.Column, from.Row] = null;
            _board[to.Column, to.Row] = originPiece;
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

            _board[rook.Position.Column, rook.Position.Row] = null;
            _board[rookPosition.Column, rookPosition.Row] = rook;
            rook.Position = rookPosition;

            _board[king.Position.Column, king.Position.Row] = null;
            _board[kingPosition.Column, kingPosition.Row] = king;
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

                foreach (Piece piece in _board)
                {
                    if (piece != null)
                        count++;
                }

                return count;
            }
        }

        public IEnumerable<Piece> AttackersFor(PieceColor color, BoardPosition position, IEnumerable<Piece> piecesToIgnore = null)
        {
            IEnumerable<Piece> result = new List<Piece>();
            ChessBoard boardWithoutIngoredPieces;

            if (piecesToIgnore != null && piecesToIgnore.Any())
            {
                boardWithoutIngoredPieces = this.Clone() as ChessBoard;

                foreach(Piece toIgnore in piecesToIgnore)
                {
                    var atPosition = GetPiece(toIgnore.Position);

                    if (atPosition != null)
                        boardWithoutIngoredPieces._board[atPosition.Position.Column, atPosition.Position.Row] = null;
                }
            }
            else
            {
                boardWithoutIngoredPieces = this;
            }

            return result
                .Concat(King.GetKingAttackersFor(boardWithoutIngoredPieces, color, position))
                .Concat(Pawn.GetPawnAttackersFor(boardWithoutIngoredPieces, color, position))
                .Concat(Knight.GetKnightAttackersFor(boardWithoutIngoredPieces, color, position))
                .Concat(Bishop.GetBishopAttackersFor(boardWithoutIngoredPieces, color, position))
                .Concat(Rook.GetRookAttackersFor(boardWithoutIngoredPieces, color, position))
                .Concat(Queen.GetQueenAttackersFor(boardWithoutIngoredPieces, color, position));
        }

        public IEnumerable<Piece> AttackersFor(Piece piece)
        {
            // Ignore current piece when checking for attackers at new position.
            return AttackersFor(piece.Color, piece.Position, new List<Piece> { piece });
        }

        public string Print()
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
                    sb.Append($"[{(_board[c, r] != null ? _board[c, r].GetSymbol() : "﹘")}]");
                }

                sb.Append("\n");
            }

            return sb.ToString();
        }

        public object Clone()
        {
            var board = new Piece[Columns, Rows];

            for (int i = 0; i < Columns; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    board[i, j] = _board[i, j];
                }
            }

            var cloned = new ChessBoard(Columns, Rows)
            {
                LastMove = this.LastMove,
                LastMovedPiece = this.LastMovedPiece,
                _board = board,
            };

            return cloned;
        }
    }
}