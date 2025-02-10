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
            if (enPassantSpot != new Spot(1, 1))
            {
                if(AddMove(ref possibleMoves, enPassantSpot)) //TODO FIX
                Debug.Log("nigga");
            }

            return possibleMoves;
        }

        public override void Move(Spot spot, bool simulation = false)
        {
            if (simulation)
            {
                base.Move(spot, simulation);
                return;
            }

            //Add en passant
            int moveDirection = isWhite ? 1 : -1;
            if (Mathf.Abs(spot.file - this.spot.file) == 2) //En passant
            {
                Spot enPassantSpot = new Spot(this.spot.rank, this.spot.file + moveDirection);
                board.SetEnPassantSpot(enPassantSpot);
            }
            //Take en passant pawn
            else
            {
                Spot enPassantSpot = board.GetEnPassantSpot();
                if (enPassantSpot != new Spot(1, 1) && enPassantSpot == spot)
                    board.DeletePiece(new Spot(enPassantSpot.rank, enPassantSpot.file - moveDirection));
            }

            base.Move(spot);
        }
    }
}
