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
            return AddMove(ref possibleMoves, new Spot(rank, file), canMoveOnEmptySquare, canTake);
        }

        protected bool AddMove(ref List<Move> possibleMoves, Spot targetSpot, bool canMoveOnEmptySquare = true, bool canTake = true)
        {
            Piece targetSquarePiece = board.pieces[targetSpot];

            if (targetSquarePiece == null)
            {
                if (canMoveOnEmptySquare) //Target square is empty 
                {
                    possibleMoves.Add(new Move(spot, targetSpot));
                    return true;
                }
            }
            else if ((canTake && isWhite != targetSquarePiece.isWhite)) //Square has enemy piece
            {
                possibleMoves.Add(new Move(spot, targetSpot));
                return true;
            }
                
            return false;
        }

        public void Move(Spot spot) => this.spot = spot;

        public void Init(Board board, bool isWhite, Spot spot)
        {
            this.board = board;
            this.isWhite = isWhite;
            this.spot = spot;
        }
    }
}
