using ChessNet.Data.Constants;
using ChessNet.Data.Enums;
using ChessNet.Data.Extensions;
using ChessNet.Data.Structs;

namespace ChessNet.Data.Models
{
    public class ChessGame
    {
        private ChessBoard _chessBoard { get; set; }
        private Player _playerWhite { get; set; }
        private Player _playerBlack { get; set; }
        private PieceColor _turn { get; set; }

        public Player CurrentPlayer => _turn == PieceColor.White ? _playerWhite : _playerBlack;
        public ChessBoard Board => _chessBoard;

        public ChessGame(IEnumerable<Piece> pieces, PieceColor turn = DefaultValues.STARTING_COLOR)
        {
            _chessBoard = new ChessBoard();
            _playerWhite = new Player(PieceColor.White);
            _playerBlack = new Player(PieceColor.Black);
            _turn = turn;

            AddPiecesRange(pieces);
        }

        public ChessGame() : this(InitialStates.DEFAULT)
        {

        }

        public void AddPiecesRange(IEnumerable<Piece> pieces)
        {
            bool isAdded = true;

            foreach (var piece in pieces)
            {
                isAdded = _chessBoard.AddPiece(piece);
                if (!isAdded) throw new InvalidOperationException($"could not add the {piece.Type} at {piece.Position.AsString()}");
            }

            _playerWhite.Pieces = _playerWhite.Pieces.Concat(pieces.Where(p => p.IsWhite));
            _playerBlack.Pieces = _playerBlack.Pieces.Concat(pieces.Where(p => !p.IsWhite));
        }

        public void AddPiece(Piece piece)
        {
            if(_chessBoard.AddPiece(piece))
            {
                if (piece.IsWhite)
                    _playerWhite.Pieces = _playerWhite.Pieces.Append(piece);
                else
                    _playerBlack.Pieces = _playerBlack.Pieces.Append(piece);
            }
            else
                throw new InvalidOperationException($"could not add the {piece.Type} at {piece.Position.AsString()}");
        }

        public bool MovePiece<T>(T piece, BoardPosition boardPosition) where T : Piece
        {
            PieceMovement nextMove;

            Piece currentPiece = _chessBoard.GetPiece(piece);
            var validMoves = currentPiece.GetMovements(_chessBoard);

            if (validMoves.TryGetAt(boardPosition, out nextMove))
            {
                var capturedPiece = _chessBoard.MovePieceAndReturnCaptured(currentPiece.Position, nextMove.Destination);

                if (nextMove.IsCapture && capturedPiece != null)
                    CurrentPlayer.Points += capturedPiece.Points;

                return true;
            }
            else
                return false;
        }
    }
}