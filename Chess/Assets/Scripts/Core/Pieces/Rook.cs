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

        public override void Move(Spot spot, bool simulation = false)
        {
            if (!simulation) DisableCastle();
            base.Move(spot);
        }

        public override void OnDelete(bool simulation = false)
        {
            if (!simulation) DisableCastle();
        }

        private void DisableCastle()
        {
            if (spot == new Spot("a1")) board.DisableCastle(CastleType.WHITE_QUEEN);
            else if (spot == new Spot("h1")) board.DisableCastle(CastleType.WHITE_KING);
            else if (spot == new Spot("a8")) board.DisableCastle(CastleType.BLACK_QUEEN);
            else if (spot == new Spot("h8")) board.DisableCastle(CastleType.BLACK_KING);
        }
    }
}
