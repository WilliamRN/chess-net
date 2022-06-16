﻿using ChessNet.Data.Constants;
using ChessNet.Data.Enums;
using ChessNet.Data.Structs;

namespace ChessNet.Data.Models.Pieces
{
    public class Queen : Piece
    {
        public Queen(PieceColor pieceColor, BoardPosition boardPosition)
            : base(pieceColor, boardPosition, PiecePoints.QUEEN)
        {

        }

        public override IEnumerable<PieceMovement> GetMovements(ChessBoard chessBoard)
        {
            return CheckLineOfPositionsBasedOnPathStep(BoardDirectionSteps.HORIZONTAL_STEP, chessBoard)
                .Concat(CheckLineOfPositionsBasedOnPathStep(BoardDirectionSteps.VERTICAL_STEP, chessBoard))
                .Concat(CheckLineOfPositionsBasedOnPathStep(BoardDirectionSteps.DIAGONAL_STEP_BACK, chessBoard))
                .Concat(CheckLineOfPositionsBasedOnPathStep(BoardDirectionSteps.DIAGONAL_STEP_FOWARD, chessBoard));
        }
    }
}
