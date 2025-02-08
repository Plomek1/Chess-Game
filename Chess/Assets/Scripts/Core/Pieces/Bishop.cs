using System.Collections.Generic;
using UnityEngine;

namespace Chess.Core
{
    public class Bishop : Piece
    {
        public override List<Move> FindPossibleMoves()
        {
            List<Move> possibleMoves = new List<Move>();

            AddLine(ref possibleMoves, new Vector2Int( 1,  1));
            AddLine(ref possibleMoves, new Vector2Int( 1, -1));
            AddLine(ref possibleMoves, new Vector2Int(-1,  1));
            AddLine(ref possibleMoves, new Vector2Int(-1, -1));

            return possibleMoves;
        }
    }
}
