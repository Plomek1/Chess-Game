using System.Collections.Generic;
using UnityEngine;

namespace Chess.Core
{
    public abstract class Piece
    {
        public bool isWhite {  get; private set; }

        protected Board board;

        protected Spot spot;

        public abstract List<Move> FindPossibleMoves();

        protected bool AddMove(ref List<Move> possibleMoves, int rank, int file, bool canMoveOnEmptySquare = true, bool canTake = true)
        {
            if (!NotationParser.ValidateCoordinates(rank, file)) return false;
            Spot targetSpot = new Spot(rank, file);
            return AddMove(ref possibleMoves, rank, file, canMoveOnEmptySquare, canTake);
        }

        protected bool AddMove(ref List<Move> possibleMoves, Spot targetSpot, bool canMoveOnEmptySquare = true, bool canTake = true)
        {
            Piece targetSquarePiece = board.pieces[targetSpot];
            if (targetSquarePiece == null || (canTake && isWhite != targetSquarePiece.isWhite)) //Check if square is empty or it has enemy piece
            {
                possibleMoves.Add(new Move(spot, targetSpot));
                return true;
            }
            return false;
        }

        public void Init(Board board, bool isWhite, Spot spot)
        {
            this.board = board;
            this.isWhite = isWhite;
            this.spot = spot;
        }
    }
}
