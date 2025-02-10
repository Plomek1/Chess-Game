using System.Collections.Generic;
using UnityEngine;

namespace Chess.Core
{
    public abstract class Piece
    {
        public Spot spot { get; private set; }
        public bool isWhite {  get; private set; }

        protected Board board;


        public abstract List<Move> FindPossibleMoves();

        protected bool AddMove(ref List<Move> possibleMoves, int rank, int file, bool canMoveOnEmptySquare = true, bool canTake = true)
        {
            if (!NotationParser.ValidateCoordinates(rank, file)) return false;
            Spot targetSpot = new Spot(rank, file);
            return AddMove(ref possibleMoves, targetSpot, canMoveOnEmptySquare, canTake);
        }

        protected bool AddMove(ref List<Move> possibleMoves, Spot targetSpot, bool canMoveOnEmptySquare = true, bool canTake = true)
        {
            Piece targetSquarePiece = board.GetPiece(targetSpot);

            if (targetSquarePiece == null)
            {
                if (canMoveOnEmptySquare) //Target square is empty 
                {
                    possibleMoves.Add(new Move(spot, targetSpot));
                    return true;
                }
            }
            else if (canTake && (isWhite != targetSquarePiece.isWhite)) //Square has enemy piece
            {
                possibleMoves.Add(new Move(spot, targetSpot));
                return true;
            }

            return false;
        }

        protected void AddLine(ref List<Move> possibleMoves, Vector2Int direction, bool canMoveOnEmptySquare = true, bool canTake = true, bool preventSelfCheck = true)
        {
            bool squareValid;
            if (!NotationParser.ValidateCoordinates(spot.rank + direction.x, spot.file + direction.y)) return;
            Spot squareSpot = new Spot(spot.rank + direction.x, spot.file + direction.y);

            do
            {
                squareValid = AddMove(ref possibleMoves, squareSpot, canMoveOnEmptySquare, canTake);

                //Checking if piece blocks the way
                if (squareValid && board.GetPiece(squareSpot) != null) squareValid = false;

                if (!NotationParser.ValidateCoordinates(squareSpot.rank + direction.x, squareSpot.file + direction.y)) return;
                squareSpot = new Spot(squareSpot.rank + direction.x, squareSpot.file + direction.y);
            }
            while (squareValid);
        }

        public virtual void Move(Spot spot, bool simulation = false) => this.spot = spot;

        public void Init(Board board, bool isWhite, Spot spot)
        {
            this.board = board;
            this.isWhite = isWhite;
            this.spot = spot;
        }
    }
}
