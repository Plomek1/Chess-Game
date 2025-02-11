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
                for (int rank = -1; rank<= 1; rank++)
                {
                    if (rank == 0 && file == 0) continue;
                    AddMove(ref possibleMoves, spot.rank + rank, spot.file + file);
                }
            }

            if ((isWhite && board.CanCastle(CastleType.WHITE_KING)) || !isWhite && board.CanCastle(CastleType.BLACK_KING)) // King side castling
            {
                Spot kingSpot = new Spot(spot.rank + 2, spot.file);
                Spot rookSpot = new Spot(spot.rank + 1, spot.file);
                if(CanCastle(kingSpot, rookSpot))
                    possibleMoves.Add(new Move(spot, kingSpot));
            }

            if ((isWhite && board.CanCastle(CastleType.WHITE_QUEEN)) || !isWhite && board.CanCastle(CastleType.BLACK_QUEEN)) // Queen side castling
            {
                Spot kingSpot = new Spot(spot.rank - 2, spot.file);
                Spot rookSpot = new Spot(spot.rank - 1, spot.file);
                if(CanCastle(kingSpot, rookSpot))
                    possibleMoves.Add(new Move(spot, kingSpot));
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

            if (isWhite)
            {
                board.DisableCastle(CastleType.WHITE_KING);
                board.DisableCastle(CastleType.WHITE_QUEEN);
            }
            else
            {
                board.DisableCastle(CastleType.BLACK_KING);
                board.DisableCastle(CastleType.BLACK_QUEEN);
            }

            if (spot.rank - this.spot.rank == 2) //King side castle
                board.MakeMoveRaw(new Move(new Spot(8, spot.file), new Spot(6, spot.file)));
            else if (spot.rank - this.spot.rank == -2) //Queen side castle
                board.MakeMoveRaw(new Move(new Spot(1, spot.file), new Spot(4, spot.file)));
        
            base.Move(spot);
        }

        private bool CanCastle(Spot kingSpot, Spot rookSpot)
        {
            if (board.GetPiece(kingSpot) != null || board.GetPiece(rookSpot) != null) return false; //Piece in the way
            if (board.IsSquareAttacked(spot, !isWhite)) return false; //King in check
            if (board.IsSquareAttacked(kingSpot, !isWhite) || board.IsSquareAttacked(rookSpot, !isWhite)) return false; //Enemy attacks target square
            return true;
        }
    }
}
