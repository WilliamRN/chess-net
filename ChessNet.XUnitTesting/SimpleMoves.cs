using ChessNet.Data.Enums;
using ChessNet.Data.Models;
using ChessNet.Data.Rules;

namespace ChessNet.XUnitTesting
{
    public class SimpleMoves
    {
        [Fact]
        public void When_BoardIsInitialized_Then_MovePeon()
        {
            ChessGame game = new ChessGame();

            var peon = game.CurrentPlayer.Pieces.First(p => p.Type == PieceType.Pawn);

            var validMove = peon.GetMoves(game.Board).First();

            game.MovePiece(peon, validMove.Destination);
        }
    }
}