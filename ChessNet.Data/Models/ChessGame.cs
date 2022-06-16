using ChessNet.Data.Constants;
using ChessNet.Data.Enums;
using ChessNet.Data.Extensions;
using ChessNet.Data.Rules;
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

        public ChessGame()
        {
            _chessBoard = new ChessBoard();
            _playerWhite = new Player(PieceColor.White);
            _playerBlack = new Player(PieceColor.Black);
            _turn = PieceColor.White;

            foreach (var piece in InitialStates.DEFAULT)
                AddPiece(piece);
        }

        public ChessGame(ChessBoard chessBoard, Player playerWhite, Player playerBlack, PieceColor turn)
        {
            _chessBoard = chessBoard;
            _playerWhite = playerWhite;
            _playerBlack = playerBlack;
            _turn = turn;
        }

        public void AddPiece(Piece piece)
        {
            if (piece.IsWhite)
                _playerWhite.Pieces = _playerWhite.Pieces.Append(piece);
            else
                _playerBlack.Pieces = _playerBlack.Pieces.Append(piece);

            _chessBoard.AddPiece(piece);
        }

        public bool MovePiece(Piece piece, BoardPosition boardPosition)
        {
            PieceMovement nextMove;

            Piece currentPiece = _chessBoard.GetPiece(piece);
            var validMoves = currentPiece.GetMoves(_chessBoard);

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