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
        public Action<Spot> PieceDeleted;

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

        public Spot GetEnPassantSpot()
        {
            if (position == null) return new Spot(1, 1);
            return position.enPassantSpot;
        }

        public void SetEnPassantSpot(Spot spot)
        {
            if(position == null) return;
            position.enPassantSpot = spot;
        }
       
        public void DeletePiece(Spot spot)
        {
            if (position == null) return;
            position.DeletePiece(spot);
            PieceDeleted?.Invoke(spot);
        }

        public void MakeMove(Move move)
        {
            if (position == null) return;

            if (position.MakeMove(move))
                PieceMoved?.Invoke(move);

            //Debug.Log(position.IsCheck(position.whiteOnMove));
            //Debug.Log(position.IsCheck(!position.whiteOnMove));
        }

        public void MakeMoveRaw(Move move)
        {
            if (position == null) return;

            position.MakeMoveRaw(move);
            PieceMoved?.Invoke(move);

            //Debug.Log(position.IsCheck(position.whiteOnMove));
            //Debug.Log(position.IsCheck(!position.whiteOnMove));
        }

        public void LoadPositionFromFEN(string fen)
        {
            //Create 64 empty squares
            Dictionary<Spot, Piece> pieces = new Dictionary<Spot, Piece>(64);
            for (int i = 1; i <= 64; i++) 
                pieces.Add(new Spot(i), null); 

            /*
                0-7:  Piece placement
                8:    Active color
                9:    Castling
                10:   En passant
                11:   Halfmove clock
                12:   Fullmove clock
            */ 
            string[] arguments = fen.Split('/', ' ');

            LoadPiecesFromFEN(ref pieces, arguments);
            bool whiteOnMove = arguments[8] == "w";

            //Castling
            CastlingData castlingData = new CastlingData
            {
                kingSideWhite  = arguments[9].Contains('K'),
                queenSideWhite = arguments[9].Contains('Q'),
                kingSideBlack  = arguments[9].Contains('k'),
                queenSideBlack = arguments[9].Contains('q'),
            };

            //En Passant
            Spot enPassantSpot = arguments[10] == "-" ? new Spot(1,1) : new Spot(arguments[10]);
            

            position = new Position();
            position.Init(pieces, whiteOnMove, castlingData, enPassantSpot);
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
