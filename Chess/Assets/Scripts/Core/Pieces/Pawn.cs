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


            //TODO: En passant

            return possibleMoves;
        }
    }
}
