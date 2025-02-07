using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Chess.Core
{
    public class Board
    {
        public Action PositionSet;
        public Action<Move> PieceMoved;

        public bool whiteOnMove { get; private set; }
        public Dictionary<Spot, Piece> pieces { get; private set; }
        public List<Move> possibleMoves { get; private set; }
        
        public void MakeMove(Move move)
        {
            if (!possibleMoves.Contains(move)) return;

            pieces[move.targetSpot] = pieces[move.startingSpot];
            pieces[move.targetSpot].Move(move.targetSpot);
            pieces[move.startingSpot] = null;
            whiteOnMove = !whiteOnMove;
            
            FindPossibleMoves();
            PieceMoved?.Invoke(move);
        }

        private void FindPossibleMoves()
        {
            possibleMoves.Clear();

            pieces.Keys.ToList().ForEach(square =>
            {
                if (pieces[square] == null || pieces[square].isWhite != whiteOnMove) return;
                possibleMoves.AddRange(pieces[square].FindPossibleMoves());
            });

            Debug.Log($"Found {possibleMoves.Count} possible moves");
        }

        public void LoadPositionFromFEN(string fen)
        {
            pieces.Keys.ToList().ForEach(square => pieces[square] = null); //Clearing position

            /*
                0-7: Piece placement
                8:   Active color
                8:   Castling
                8:   En passant
                8:   Halfmove clock
                8:   Fullmove clock
            */ 
            string[] arguments = fen.Split('/', ' ');

            LoadPieces(arguments);
            whiteOnMove = arguments[8] == "w";

            FindPossibleMoves();
            PositionSet?.Invoke();
        }

        private void LoadPieces(string[] arguments )
        {
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
                    piece.Init(this, pieceWhite, new Spot(rank, file));
                    pieces[new Spot(rank, file)] = piece;

                    rank++;
                }
            }
        }

        public Board()
        {
            pieces = new Dictionary<Spot, Piece>(64);
            possibleMoves = new List<Move>();

            for (int i = 1; i <= 64; i++)
                pieces.Add(new Spot(i), null);
        }
    }
}
