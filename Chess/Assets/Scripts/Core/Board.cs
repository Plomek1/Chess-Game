using System.Collections.Generic;
using UnityEngine;


namespace Chess.Core
{
    public class Board
    {
        private Dictionary<Spot, Piece> pieces;
        
        public Board()
        {
            pieces = new Dictionary<Spot, Piece>(64);
            for (int file = 1; file <= 8; file++)
            {
                for (int rank = 1; rank <= 8; rank++)
                {
                    pieces.Add(new Spot(file, rank), new Piece());
                }
            }
        }

        public Piece GetPiece(Spot spot) => pieces[spot];
    }
}
