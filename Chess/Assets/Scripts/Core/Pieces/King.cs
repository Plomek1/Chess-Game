using System.Collections.Generic;

namespace Chess.Core
{
    public class King : Piece
    {
        public override List<Move> FindPossibleMoves()
        {
            List<Move> possibleMoves = new List<Move>();
            for (int file = -1; file <= 1; file++)
            {
                //TODO: Castling
                for (int rank = -1; rank<= 1; rank++)
                {
                    if (rank == 0 && file == 0) continue;
                    AddMove(ref possibleMoves, spot.rank + rank, spot.file + file);
                }
            }

            return possibleMoves;
        }
    }
}
