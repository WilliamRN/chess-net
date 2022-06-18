using ChessNet.Data.Enums;
using ChessNet.Data.Models;

namespace ChessNet.XUnitTesting.GameExecution
{
    public class StateChanges
    {
        [Fact]
        public void When_BoardIsCreatedEmpty_Then_IsSetupPhase()
        {
            var pieces = new List<Piece>();
            var game = new ChessGame(pieces);

            Assert.Equal(GameStates.Setup, game.GameState);
        }

        [Fact]
        public void When_BoardIsCreatedWithDefaults_Then_IsReadyToStart()
        {
            var game = new ChessGame();

            Assert.Equal(GameStates.Start, game.GameState);
        }
    }
}
