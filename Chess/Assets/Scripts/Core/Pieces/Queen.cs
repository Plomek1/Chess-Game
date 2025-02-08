using System.Collections.Generic;
using UnityEngine;

namespace Chess.Core
{
    public class Queen : Piece
    {
        public override List<Move> FindPossibleMoves()
        {
            List<Move> possibleMoves = new List<Move>();

            //Lines
            AddLine(ref possibleMoves, Vector2Int.up);
            AddLine(ref possibleMoves, Vector2Int.right);
            AddLine(ref possibleMoves, Vector2Int.down);
            AddLine(ref possibleMoves, Vector2Int.left);

            //Diagonals
            AddLine(ref possibleMoves, new Vector2Int(1, 1));
            AddLine(ref possibleMoves, new Vector2Int(1, -1));
            AddLine(ref possibleMoves, new Vector2Int(-1, 1));
            AddLine(ref possibleMoves, new Vector2Int(-1, -1));

            return possibleMoves;
        }
    }
}
