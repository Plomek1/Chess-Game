using System.Collections.Generic;

namespace Chess.Core
{
    public class Knight : Piece
    {
        public override List<Move> FindPossibleMoves()
        {
            List<Move> possibleMoves = new List<Move>();

            AddMove(ref possibleMoves, spot.rank + 2, spot.file + 1);
            AddMove(ref possibleMoves, spot.rank + 2, spot.file - 1);
            AddMove(ref possibleMoves, spot.rank - 2, spot.file + 1);
            AddMove(ref possibleMoves, spot.rank - 2, spot.file - 1);

            AddMove(ref possibleMoves, spot.rank + 1, spot.file + 2);
            AddMove(ref possibleMoves, spot.rank - 1, spot.file + 2);
            AddMove(ref possibleMoves, spot.rank + 1, spot.file - 2);
            AddMove(ref possibleMoves, spot.rank - 1, spot.file - 2);

            return possibleMoves;
        }
    }
}
