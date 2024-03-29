﻿using ChessNet.Data.Constants;
using ChessNet.Data.Enums;
using ChessNet.Data.Structs;
using System.Diagnostics;

namespace ChessNet.Data.Models.Pieces
{
    [DebuggerDisplay("{Color} {this.GetType().Name} at {Position} inPlay:{IsInChessBoard}")]
    public class King : Piece
    {
        public King(PieceColor pieceColor, BoardPosition boardPosition)
            : base(pieceColor, boardPosition, PiecePoints.KING)
        {

        }

        public override string Symbol => Color == PieceColor.White ? "♔" : "♚";

        public override PieceType PieceType => PieceType.King;

        public override IEnumerable<Movement> GetMovements()
        {
            if (!IsInChessBoard) yield break;

            foreach (var validOffset in MoveOffsets.KING)
            {
                if (TryGetValidPieceMovement(Board, Position, validOffset, Color, out Movement value))
                {
                    if (!Board.AttackersFor(this, value.Destination).Any())
                        yield return value;
                }
            }

            foreach (var m in Castling())
                yield return m;
        }

        private IEnumerable<Movement> Castling()
        {
            // The king is not in check.
            if (Board.AttackersFor(this).Any())
                yield break;

            // The king has not moved.
            if (!IsFirstMove)
                yield break;

            // The rooks have not previously moved, and the rooks are on the same row.
            var rooks = Board
                .GetPieces(Color)
                .Where(p =>
                    p is Rook &&
                    p.IsFirstMove &&
                    p.Position.Row == Position.Row);

            foreach (var rook in rooks)
            {
                // Find the direction to the rook.
                int diff = Position.Column - rook.Position.Column;
                int step = diff > 0 ? -1 : 1;
                bool isPathOcuppied = false;

                // The squares between the king and the rook are unoccupied.
                for (int i = Position.Column
                    ; step > 0 ? i <= rook.Position.Column : i >= rook.Position.Column
                    ; i += step)
                {
                    if (i == Position.Column || i == rook.Position.Column)
                        continue;

                    if (Board.GetPiece(new BoardPosition(i, Position.Row)) != null)
                        isPathOcuppied = true;
                }

                if (!isPathOcuppied)
                {
                    BoardPosition rookCastlingPosition = new(Position.Column + step, Position.Row);
                    BoardPosition kingCastlingPosition = new(Position.Column + (step * 2), Position.Row);

                    // The king does not cross over or end up on a square attacked by an opposing piece.
                    if (!Board.AttackersFor(rook, rookCastlingPosition).Any() &&
                        !Board.AttackersFor(this, kingCastlingPosition).Any())
                    {
                        yield return new Movement(kingCastlingPosition, rook, isCastling: true);
                    }
                }
            }
        }

        public override IEnumerable<Piece> GetAttackersFor(ChessBoard chessBoard, PieceColor color, BoardPosition position)
        {
            Piece attacker;
            var attackerColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;
            BoardPosition piecePosition;

            foreach (var validOffset in MoveOffsets.KING)
            {
                piecePosition = position.GetOffset(validOffset);
                attacker = chessBoard.IsValidPosition(piecePosition) ? chessBoard.GetPiece(piecePosition) : null;

                if (attacker != null && attacker is King && attacker.Color == attackerColor)
                    yield return attacker as King;
            }
        }
    }
}
