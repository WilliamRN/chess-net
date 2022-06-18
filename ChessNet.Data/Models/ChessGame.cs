using ChessNet.Data.Constants;
using ChessNet.Data.Enums;
using ChessNet.Data.Extensions;
using ChessNet.Data.Models.Pieces;
using ChessNet.Data.Structs;

namespace ChessNet.Data.Models
{
    public class ChessGame
    {
        public ChessBoard Board { get; private set; }
        public GameStates GameState { get; private set; }

        private Player _playerWhite { get; set; }
        private Player _playerBlack { get; set; }
        private PieceColor _turn { get; set; }

        public Player CurrentPlayer => _turn == PieceColor.White ? _playerWhite : _playerBlack;

        public ChessGame(IEnumerable<Piece> pieces, PieceColor turn = DefaultValues.STARTING_COLOR)
        {
            Board = new ChessBoard();
            GameState = GameStates.Setup;
            _playerWhite = new Player(PieceColor.White, () => Board.GetPieces(PieceColor.White));
            _playerBlack = new Player(PieceColor.Black, () => Board.GetPieces(PieceColor.Black));
            _turn = turn;

            AddPiecesRange(pieces);
        }

        public ChessGame() : this(InitialBoardPlacements.DEFAULT)
        {

        }

        public void AddPiecesRange(IEnumerable<Piece> pieces)
        {
            foreach (var piece in pieces)
                AddPiece(piece);

            if (Board.PieceCount > 0 && 
                GameState == GameStates.Setup &&
                _playerBlack.Pieces.Any(p => p is King) &&
                _playerWhite.Pieces.Any(p => p is King))
            {
                GameState = GameStates.Start;
            }
        }

        public void AddPiece(Piece piece)
        {
            if(!Board.AddPiece(piece))
                throw new InvalidOperationException($"could not add the {piece.GetType().Name} at {piece.Position.AsString()}");
        }

        public bool MovePiece<T>(T piece, BoardPosition boardPosition) where T : Piece
        {
            PieceMovement nextMove;

            if (piece == null)
                throw new ArgumentNullException(nameof(piece), "piece cannot be empty");

            if (boardPosition.IsDefault)
                throw new ArgumentNullException(nameof(boardPosition), "position cannot be empty");

            if (!piece.IsInChessBoard)
                throw new InvalidOperationException("piece is not on the board");

            if (piece.Color != CurrentPlayer.Color)
                throw new InvalidOperationException($"invalid player piece, expected a {CurrentPlayer.Color} piece but got a {piece.Color} {piece.GetType().Name}");

            var validMoves = piece.GetMovements();

            if (validMoves.TryMoveTo(boardPosition, out nextMove))
            {
                var capturedPiece = Board.MovePieceAndReturnCaptured(piece, nextMove);

                if (nextMove.IsCaptureFor(CurrentPlayer.Color))
                    CurrentPlayer.Points += capturedPiece.Points;

                if (piece is Pawn && (piece as Pawn).IsPromotingToQueen())
                    PromotePawnToQueen(piece as Pawn);

                _turn = CurrentPlayer.Color == PieceColor.Black ? PieceColor.White : PieceColor.Black;

                return true;
            }
            else
                return false;
        }

        private void PromotePawnToQueen(Pawn pawn)
        {
            Board.RemovePiece(pawn);
            Board.AddPiece(new Queen(pawn.Color, pawn.Position));
        }

        private void CheckGameState()
        {
            throw new NotImplementedException();
        }

        private void GameEnd()
        {
            throw new NotImplementedException();
        }
    }
}