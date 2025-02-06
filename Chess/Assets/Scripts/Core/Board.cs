using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Chess.Core
{
    public class Board
    {
        public Action PositionSet;
        
        public Dictionary<Square, Piece> pieces { get; private set; }

        
        public Board()
        {
            pieces = new Dictionary<Square, Piece>(64);
            for (int i = 1;  i <= 64; i++) 
                pieces.Add(new Square(i), null);

        }

        public void LoadPositionFromFEN(string fen)
        {
            pieces.Keys.ToList().ForEach(square => pieces[square] = null);

            /*
                0-7: Piece placement
                8:   Active color
                8:   Castling
                8:   En passant
                8:   Halfmove clock
                8:   Fullmove clock
            */ 
            string[] arguments = fen.Split('/', ' ');

            for (int file = 1; file <= 8; file++)
            {
                string filePieces = arguments[8 - file];

                int rank = 1;
                foreach (char pieceChar in filePieces)
                {
                    //If next char is int, skip squares
                    if (int.TryParse(pieceChar.ToString(), out int emptySquares))
                    {
                        rank += emptySquares;
                        continue;
                    }

                    bool pieceWhite = pieceChar == char.ToUpper(pieceChar);

                    Piece piece = NotationParser.NotationToPiece(pieceChar);
                    piece.Init(this, pieceWhite, new Square(rank, file));
                    pieces[new Square(rank, file)] = piece;

                    rank++;
                }
            }

            PositionSet?.Invoke();
        }
    }
}
