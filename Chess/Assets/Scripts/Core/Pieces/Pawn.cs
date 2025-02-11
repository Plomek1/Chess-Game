using System.Collections.Generic;
using UnityEngine;

namespace Chess.Core
{
    public class Pawn : Piece
    {
        public override List<Move> FindPossibleMoves()
        {
            List<Move> possibleMoves = new List<Move>();

            int moveDirection = isWhite ? 1 : -1;
            bool firstMove = (isWhite && spot.file == 2) || (!isWhite && spot.file == 7);

            //Normal moves
            if (AddMove(ref possibleMoves, spot.rank, spot.file + moveDirection, canTake: false) && firstMove)
                AddMove(ref possibleMoves, spot.rank, spot.file + moveDirection * 2, canTake: false);
            
            //Takes
            AddMove(ref possibleMoves, spot.rank + 1, spot.file + moveDirection, canMoveOnEmptySquare: false);
            AddMove(ref possibleMoves, spot.rank - 1, spot.file + moveDirection, canMoveOnEmptySquare: false);

            Spot enPassantSpot = board.GetEnPassantSpot();

            if (enPassantSpot != new Spot(1, 1) 
                && Mathf.Abs(spot.rank - enPassantSpot.rank) == 1 && spot.file + moveDirection == enPassantSpot.file) //Make sure pawn is in the right position
            {
                AddMove(ref possibleMoves, enPassantSpot);
                    //Debug.Log(possibleMoves.Count);

            }

            return possibleMoves;
        }

        public override void Move(Spot spot, bool overrideEnPassant = true)
        {
            if (!overrideEnPassant)
            {
                base.Move(spot, overrideEnPassant);
                return;
            }
            
            int moveDirection = isWhite ? 1 : -1;
            bool doublePawnMove = Mathf.Abs(spot.file - this.spot.file) == 2;
            
            if (doublePawnMove)
            {
                Spot enPassantSpot = new Spot(this.spot.rank, this.spot.file + moveDirection);
                board.SetEnPassantSpot(enPassantSpot);
            }
            else //Check if player just did en passant
            {
                Spot enPassantSpot = board.GetEnPassantSpot();
                if (enPassantSpot != new Spot(1, 1) && enPassantSpot == spot)
                    board.DeletePiece(new Spot(enPassantSpot.rank, enPassantSpot.file - moveDirection));
            }

            base.Move(spot, !doublePawnMove);
        }
    }
}
