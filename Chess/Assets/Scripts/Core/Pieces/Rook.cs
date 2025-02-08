using System.Collections.Generic;
using UnityEngine;

namespace Chess.Core
{
    public class Rook : Piece
    {
        public override List<Move> FindPossibleMoves()
        {
            List<Move> possibleMoves = new List<Move>();

            AddLine(ref possibleMoves, Vector2Int.up);
            AddLine(ref possibleMoves, Vector2Int.right);
            AddLine(ref possibleMoves, Vector2Int.down);
            AddLine(ref possibleMoves, Vector2Int.left);

            return possibleMoves;
        }
    }
}
