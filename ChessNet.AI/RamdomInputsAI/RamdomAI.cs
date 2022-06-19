using ChessNet.AI.Interfaces;
using ChessNet.Data.Enums;
using ChessNet.Data.Models;
using ChessNet.Data.Structs;
using ChessNet.Utilities.Extensions;
using System.Security.Cryptography;

namespace ChessNet.AI.RamdomInputsAI
{
    // DecisionLogic
    //   ► Pick random move.
    //      - List all moves and all pieces.
    //   ► Based on board pieces and moves available.
    //      - Need to know my color.

    // Interactions
    //   ► Move piece from position 'from' to 'to'

    public class RamdomAI : IPlayerAI
    {
        private ChessGame _game { get; set; }
        private PieceColor _myColor { get; set; }
        private IEnumerable<Piece> _myPieces => _game.Board.GetPieces(_myColor);

        public RamdomAI(ChessGame chessGame, PieceColor aiPieceColor)
        {
            _game = chessGame;
            _myColor = aiPieceColor;
        }

        public PieceMovement GetNextMove()
        {
            Piece movingPiece = null;
            PieceMovement pieceMovement = new();
            Movement move = new();

            var currentPieces = _myPieces.ToList();

            if (!currentPieces.IsEmpty() && !_game.IsFinished)
            {
                while(move.IsDefault)
                {
                    movingPiece = currentPieces[RandomNumberGenerator.GetInt32(0, currentPieces.Count)];

                    var availableMoves = movingPiece.GetMovements().ToList();

                    if (availableMoves.Any())
                    {
                        move = availableMoves[RandomNumberGenerator.GetInt32(0, availableMoves.Count)];
                    }
                }
            }

            if (!move.IsDefault && movingPiece != null)
            {
                pieceMovement = new(movingPiece, move.Destination, move.PieceAtDestination);
            }

            return pieceMovement;
        }
    }
}