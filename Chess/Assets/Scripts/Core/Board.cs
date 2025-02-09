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

        private Position position;

        public Dictionary<Spot, Piece> GetAllPieces()
        {
            if (position == null) return null;
            return position.pieces;
        }

        public Piece GetPiece(Spot spot)
        {
            if (position == null) return null;
            return position.pieces[spot];
        }

        public List<Move> GetPossibleMoves()
        {
            if (position == null) return null;
            return position.possibleMoves;
        }

        public bool IsWhiteOnMove()
        {
            if (position == null) return false;
            return position.whiteOnMove;
        }

        public void MakeMove(Move move)
        {
            if (position == null) return;

            if (position.MakeMove(move))
                PieceMoved?.Invoke(move);

            Debug.Log(position.IsCheck(position.whiteOnMove));
            Debug.Log(position.IsCheck(!position.whiteOnMove));
        }

        public void LoadPositionFromFEN(string fen)
        {
            //Create 64 empty squares
            Dictionary<Spot, Piece> pieces = new Dictionary<Spot, Piece>(64);
            for (int i = 1; i <= 64; i++) 
                pieces.Add(new Spot(i), null); 

            /*
                0-7: Piece placement
                8:   Active color
                8:   Castling
                8:   En passant
                8:   Halfmove clock
                8:   Fullmove clock
            */ 
            string[] arguments = fen.Split('/', ' ');

            LoadPiecesFromFEN(ref pieces, arguments);
            bool whiteOnMove = arguments[8] == "w";

            position = new Position();
            position.Init(pieces, whiteOnMove);
            PositionSet?.Invoke();
        }

        private void LoadPiecesFromFEN(ref Dictionary<Spot, Piece> pieces, string[] arguments)
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
        public Board() { }
    }
}
