using ChessNet.Data.Enums;
using ChessNet.Data.Interfaces;
using ChessNet.Data.Models;
using ChessNet.Data.Models.Pieces;
using ChessNet.Data.Structs;
using ChessNet.Utilities.Extensions;
using System.Security.Cryptography;

namespace ChessNet.AI.RamdomInputsAI
{
    public class RamdomAI : IPlayer
    {
        private string _name { get; set; }
        private ChessGame _game { get; set; }
        private PieceColor _myColor { get; set; }
        private IEnumerable<Piece> _myPieces => _game.Board.GetPieces(_myColor);

        public RamdomAI(ChessGame chessGame, PieceColor aiPieceColor, string name = "Computer")
        {
            _game = chessGame;
            _name = name;
            _myColor = aiPieceColor;
        }

        public string GetName() => _name;

        public PieceColor GetColor() => _myColor;

        public PieceMovement GetNextMove()
        {
            Piece movingPiece = null;
            PieceMovement pieceMovement = default;
            Movement move = default;

            var currentPieces = _myPieces.ToList();

            if (currentPieces.Count == 1 && currentPieces.First() is King)
                return PieceMovement.Forfeit;

            if (!currentPieces.IsEmpty() && !_game.IsFinished)
            {
                while(move.IsDefault)
                {
                    movingPiece = currentPieces[RandomNumberGenerator.GetInt32(0, currentPieces.Count)];

                    var availableMoves = movingPiece.GetMovements().ToList();

                    if (availableMoves.Any(m => m.IsCaptureFor(_myColor)))
                    {
                        move = availableMoves.First(m => m.IsCaptureFor(_myColor));
                    }
                    else if (availableMoves.Any())
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