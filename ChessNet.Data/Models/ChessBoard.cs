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
        public Movement LastMove { get; private set; }
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
                piece.Board = this;
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
                    piece.Board = currentPiece.Board = null;
                    return true;
                }
            }

            return false;
        }

        public IEnumerable<Piece> GetPieces()
        {
            return _board
                .Cast<Piece>()
                .Where(p => p != null);
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

        public Piece MovePieceAndReturnCaptured<T>(T piece, Movement pieceMovement) where T : Piece
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
                destinationPiece.Board = null;

            LastMove = pieceMovement;
            LastMovedPiece = piece;

            return destinationPiece;
        }

        public Piece MoveEnPassantAndReturnCaptured(Pawn piece, Movement pieceMovement)
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

            capturedPiece.Board = null;

            LastMove = pieceMovement;
            LastMovedPiece = piece;

            return capturedPiece;
        }

        public Piece MoveCastling(King king, Movement pieceMovement)
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

        public Movement MoveTo(BoardPosition position)
        {
            if (IsValidPosition(position))
            {
                var piece = GetPiece(position);
                return new Movement(position, piece);
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

        public IEnumerable<Piece> AttackersFor(Piece piece, BoardPosition position)
        {
            IEnumerable<Piece> result = new List<Piece>();
            ChessBoard previewBoard;

            if (piece.Position != position)
                previewBoard = GetPreviewBoardFor(piece, position);
            else
                previewBoard = this;

            var oposingPieceTypes = previewBoard
                .GetPieces(piece.Color == PieceColor.White ? PieceColor.Black : PieceColor.White)
                .GroupBy(p => p.PieceType, (key, g) => g.OrderBy(e => e.PieceType).FirstOrDefault())
                .Where(p => p != null);

            foreach (var pieceType in oposingPieceTypes)
            {
                foreach (var attacker in pieceType.GetAttackersFor(previewBoard, piece.Color, position))
                {
                    yield return attacker;
                }
            }

            yield break;
        }

        public IEnumerable<Piece> AttackersFor(Piece piece) => 
            AttackersFor(piece, piece.Position);

        public ChessBoard GetPreviewBoardFor(Piece piece, BoardPosition position)
        {
            ChessBoard previewBoard;
            previewBoard = this.Clone() as ChessBoard;

            previewBoard._board[piece.Position.Column, piece.Position.Row] = null;
            previewBoard._board[position.Column, position.Row] = piece;

            return previewBoard;
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
                    sb.Append($"[{(_board[c, r] != null ? _board[c, r].Symbol : "﹘")}]");
                }

                sb.Append("\n");
            }

            return sb.ToString();
        }

        public Piece[,] GetBoard()
        {
            var board = new Piece[Columns, Rows];

            for (int i = 0; i < Columns; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    board[i, j] = _board[i, j];
                }
            }

            return board;
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