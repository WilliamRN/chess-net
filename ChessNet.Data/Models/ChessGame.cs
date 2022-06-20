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
        public GameStates State { get; private set; }

        private Player _playerWhite { get; set; }
        private Player _playerBlack { get; set; }
        private PieceColor _turn { get; set; }

        public Player CurrentPlayer => _turn == PieceColor.White ? _playerWhite : _playerBlack;

        public int WhiteScore => _playerWhite.Points;
        public int BlackScore => _playerBlack.Points;
        public bool IsFinished => GameEndStates.LIST.Contains(State);

        public ChessGame(IEnumerable<Piece> pieces, PieceColor turn = DefaultValues.STARTING_COLOR)
        {
            Board = new ChessBoard();
            State = GameStates.Setup;
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
                State == GameStates.Setup &&
                _playerBlack.Pieces.Any(p => p is King) &&
                _playerWhite.Pieces.Any(p => p is King))
            {
                State = GameStates.Start;
            }
        }

        public void AddPiece(Piece piece)
        {
            if(!Board.AddPiece(piece))
                throw new InvalidOperationException($"could not add the {piece.GetType().Name} at {piece.Position.AsString()}");

            piece.SetStateGetter(() => State);
        }

        public bool MovePiece<T>(T piece, BoardPosition boardPosition) where T : Piece
        {
            Movement nextMove;

            if (piece == null)
                throw new ArgumentNullException(nameof(piece), "piece cannot be empty");

            if (boardPosition.IsDefault)
                throw new ArgumentNullException(nameof(boardPosition), "position cannot be empty");

            if (!piece.IsInChessBoard)
                throw new InvalidOperationException("piece is not on the board");

            if (piece.Color != CurrentPlayer.Color)
                throw new InvalidOperationException($"invalid player piece, expected a {CurrentPlayer.Color} piece but got a {piece.Color} {piece.GetType().Name}");

            if (IsFinished)
                throw new InvalidOperationException($"cannot process move, game is finished on state {State} for {CurrentPlayer.Color}");

            var validMoves = piece.GetMovements();

            if (validMoves.TryMoveTo(boardPosition, out nextMove))
            {
                var capturedPiece = Board.MovePieceAndReturnCaptured(piece, nextMove);

                if (nextMove.IsCaptureFor(CurrentPlayer.Color))
                    CurrentPlayer.Points += capturedPiece.Points;

                if (piece is Pawn && (piece as Pawn).IsPromotingToQueen())
                    PromotePawnToQueen(piece as Pawn);

                _turn = CurrentPlayer.Color == PieceColor.Black ? PieceColor.White : PieceColor.Black;

                CheckGameState();

                return true;
            }
            else
                return false;
        }

        public bool Move(BoardPosition origin, BoardPosition destiny)
        {
            Piece piece = Board.GetPiece(origin);
            return MovePiece(piece, destiny);
        }

        public bool Move(PieceMovement move)
        {
            if (move.IsDefault)
                throw new ArgumentException("move cannot be default", nameof(move));

            if (move.IsSurrender) 
                return SetGameStateSurrender();

            Piece piece = move.FromPiece != null 
                ? move.FromPiece 
                : Board.GetPiece(move.FromPosition);

            return MovePiece(piece, move.ToPosition);
        }

        private void PromotePawnToQueen(Pawn pawn)
        {
            Board.RemovePiece(pawn);
            Board.AddPiece(new Queen(pawn.Color, pawn.Position));
        }

        private bool SetGameStateSurrender()
        {
            State = GameStates.Surrender;
            GameEnd();
            return true;
        }

        private void CheckGameState()
        {
            var currentPlayerKing = CurrentPlayer.Pieces.FirstOrDefault(p => p is King) as King;

            if (currentPlayerKing == null)
            {
                State = GameStates.End;
                GameEnd();
            }
            else
            {
                State = GameStates.Playing;

                // Game state must be set to 'Check' to look for availables moves under check condition.
                if (Board.AttackersFor(currentPlayerKing).Any())
                {
                    State = GameStates.Check;
                }

                if (!IsAnyMoveAvailableForCurrentPlayer())
                {
                    State = GameStates.CheckMate;
                    GameEnd();
                }
            }
        }

        // A king that is under attack is said to be in check, and the player in check
        // must immediately remedy the situation. There are three possible ways to remove
        // the king from check:

        //      The king is moved to an adjacent non - threatened square.A king cannot castle
        //      to get out of check. A king can capture an adjacent enemy piece if that piece
        //      is not protected by another enemy piece.

        //      A piece is interposed between the king and the attacking piece to break the
        //      line of threat (not possible when the attacking piece is a knight or pawn, or
        //      when in double check).

        //      The attacking piece is captured(not possible when in double check, unless the
        //      king captures).

        // If none of the three options are available, the player's king has been checkmated,
        // and the player loses the game.

        // At amateur levels, when placing the opponent's king in check, it is common to
        // announce "check", but this is not required by the rules of chess.

        private bool IsAnyMoveAvailableForCurrentPlayer()
        {
            foreach (Piece p in CurrentPlayer.Pieces)
            {
                if (p.GetMovements().Any())
                    return true;
            }

            return false;
        }

        private void GameEnd()
        {
            // TODO: ¯\_(ツ)_/¯
        }
    }
}