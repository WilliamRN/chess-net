using ChessNet.Data.Enums;
using ChessNet.Data.Models;
using ChessNet.Data.Models.Pieces;
using ChessNet.Data.Structs;

namespace ChessNet.XUnitTesting.GameExecution
{
    public class Gameplay
    {
        [Fact]
        public void When_KingIsCaptured_Then_GameEnds()
        {
            List<Piece> pieces = new()
            {
                new King(PieceColor.White, new BoardPosition("E3")),
                new King(PieceColor.Black, new BoardPosition("E4")),
            };

            ChessGame game = new(pieces);

            var wKing = game.Board.GetPiece(new BoardPosition("E3")) as King;
            var bKing = game.Board.GetPiece(new BoardPosition("E4")) as King;

            game.MovePiece(wKing, new BoardPosition("E4"));

            Assert.Equal(GameStates.End, game.State);
        }
    }
}
