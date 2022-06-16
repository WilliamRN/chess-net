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
            _playerWhite = new Player(PieceColor.White, () => _chessBoard.GetPieces(PieceColor.White));
            _playerBlack = new Player(PieceColor.Black, () => _chessBoard.GetPieces(PieceColor.Black));
            _turn = turn;

            AddPiecesRange(pieces);
        }

        public ChessGame() : this(InitialStates.DEFAULT)
        {

        }

        public void AddPiecesRange(IEnumerable<Piece> pieces)
        {
            foreach (var piece in pieces)
                AddPiece(piece);
        }

        public void AddPiece(Piece piece)
        {
            if(!_chessBoard.AddPiece(piece))
                throw new InvalidOperationException($"could not add the {piece.GetType().Name} at {piece.Position.AsString()}");
        }

        public bool MovePiece<T>(T piece, BoardPosition boardPosition) where T : Piece
        {
            PieceMovement nextMove;

            Piece currentPiece = _chessBoard.GetPiece(piece);

            if (currentPiece.Color != CurrentPlayer.Color)
                throw new InvalidOperationException($"invalid player piece, expected a {CurrentPlayer.Color} piece but got a {piece.Color} {piece.GetType().Name}");

            var validMoves = currentPiece.GetMovements(_chessBoard);

            if (validMoves.TryMoveTo(boardPosition, out nextMove))
            {
                var capturedPiece = _chessBoard.MovePieceAndReturnCaptured(currentPiece.Position, nextMove.Destination);

                if (nextMove.IsCaptureFor(CurrentPlayer.Color))
                    CurrentPlayer.Points += capturedPiece.Points;

                _turn = CurrentPlayer.Color == PieceColor.Black ? PieceColor.White : PieceColor.Black;

                return true;
            }
            else
                return false;
        }
    }
}