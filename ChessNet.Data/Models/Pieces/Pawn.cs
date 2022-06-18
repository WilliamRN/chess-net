﻿using ChessNet.Data.Constants;
using ChessNet.Data.Enums;
using ChessNet.Data.Structs;
using System.Diagnostics;

namespace ChessNet.Data.Models.Pieces
{
    [DebuggerDisplay("{Color} {this.GetType().Name} at {Position} inPlay:{IsInChessBoard}")]
    public class Pawn : Piece
    {
        public bool IsLastMoveTwoSpaces { get; internal set; }

        private int PawnStep => PawnStepByColor(Color);

        internal static int PawnStepByColor(PieceColor color) => color == PieceColor.White ? 1 : -1;

        public Pawn(PieceColor pieceColor, BoardPosition boardPosition) 
            : base(pieceColor, boardPosition, PiecePoints.PAWN)
        {

        }

        public override string GetSymbol()
        {
            return Color == PieceColor.White ? "♙" : "♟";
        }

        public override IEnumerable<PieceMovement> GetMovements()
        {
            if (!IsInChessBoard) yield break;

            BoardPosition position;
            PieceMovement move;
            var isPieceAhead = false;

            // Can move but not capture ahead
            position = Position.GetOffset(0, PawnStep);
            move = ChessBoard.MoveTo(position);

            if (IsValidMove(move))
                yield return move;
            else
                isPieceAhead = true;

            // On first move, if the path is clear, its valid to move two spaces.
            if (IsFirstMove && !isPieceAhead)
            {
                position = Position.GetOffset(0, PawnStep * 2);
                move = ChessBoard.MoveTo(position);

                if(IsValidMove(move))
                    yield return move;
            }

            // Can only capture on imediate diagonals
            position = Position.GetOffset(1, PawnStep);
            move = ChessBoard.MoveTo(position);
            if (IsValidCapture(move)) yield return move;

            position = Position.GetOffset(-1, PawnStep);
            move = ChessBoard.MoveTo(position);
            if (IsValidCapture(move)) yield return move;

            // En passant check
            move = EnPassant();
            if (!move.IsDefault) yield return move;
        }

        private PieceMovement EnPassant()
        {
            // Last moved piece must be a Pawn.
            if (ChessBoard.LastMovedPiece != null && ChessBoard.LastMovedPiece is Pawn)
            {
                Pawn lastMovedPiece = ChessBoard.LastMovedPiece as Pawn;

                // Captured Pawn must be adjacent.
                if (lastMovedPiece.Position.Row == Position.Row &&
                    (lastMovedPiece.Position.Column == Position.Column - 1 ||
                    lastMovedPiece.Position.Column == Position.Column + 1))
                {
                    // Capturing Pawn must have moved two spaces.
                    if (lastMovedPiece.IsLastMoveTwoSpaces)
                    {
                        // En passant move destinarion must be empty.
                        BoardPosition capturePosition = new BoardPosition(
                            lastMovedPiece.Position.Column,
                            Position.Row + PawnStep);

                        if (ChessBoard.GetPiece(capturePosition) == null)
                        {
                            return new PieceMovement(capturePosition, lastMovedPiece, isEnPassant: true);
                        }
                    }
                }
            }

            return default;
        }

        private bool IsValidMove(PieceMovement move)
        {
            return move.IsValidPosition && move.PieceAtDestination is null;
        }

        private bool IsValidCapture(PieceMovement move)
        {
            return move.IsValidPosition && move.IsCaptureFor(Color);
        }

        public bool IsPromotingToQueen()
        {
            return Position.Row == ChessBoard.Rows - 1 || Position.Row == 0;
        }

        internal static IEnumerable<Pawn> GetPawnAttackersFor(ChessBoard chessBoard, PieceColor color, BoardPosition position)
        {
            Piece attacker;
            var attackerColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;
            var attackerStep = PawnStepByColor(attackerColor);
            BoardPosition piecePosition;

            // Can only capture on imediate diagonals
            piecePosition = position.GetOffset(1, -attackerStep);
            attacker = chessBoard.IsValidPosition(piecePosition) ? chessBoard.GetPiece(piecePosition) : null;

            if (attacker != null && attacker is Pawn && attacker.Color == attackerColor)
                yield return attacker as Pawn;

            piecePosition = position.GetOffset(-1, -attackerStep);
            attacker = chessBoard.IsValidPosition(piecePosition) ? chessBoard.GetPiece(piecePosition) : null;

            if (attacker != null && attacker is Pawn && attacker.Color == attackerColor)
                yield return attacker as Pawn;
        }
    }
}
