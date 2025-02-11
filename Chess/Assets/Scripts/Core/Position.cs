using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chess.Core
{
    public class Position
    {
        public Dictionary<Spot, Piece> pieces { get; private set; }
        public bool whiteOnMove { get; private set; }
        public Dictionary<CastleType, bool> castlingData { get; private set; }
        public Spot enPassantSpot; //a1 if there is no en passant move

        public List<Move> possibleMoves { get { return whiteOnMove ? whiteMoves : blackMoves; }}
        public Spot whiteKingSpot { get; private set; }
        public Spot blackKingSpot { get; private set; }

        private List<Move> whiteMoves;
        private List<Move> blackMoves;

        public void Init(Dictionary<Spot, Piece> pieces, bool whiteOnMove, Dictionary<CastleType, bool> castlingData, Spot enPassantSpot)
        {
            this.pieces = pieces;
            this.whiteOnMove = whiteOnMove;
            this.castlingData = castlingData;
            this.enPassantSpot = enPassantSpot;

            whiteMoves = new List<Move>();
            blackMoves = new List<Move>();
            UpdatePosition();
        }

        public bool MakeMove(Move move)
        {
            if (!possibleMoves.Contains(move)) return false;

            MakeMoveRaw(move);

            whiteOnMove = !whiteOnMove;
            UpdatePosition();
            return true;
        }
        
        public void MakeMoveRaw(Move move, bool simulation = false)
        {
            pieces[move.targetSpot]?.OnDelete(simulation);
            pieces[move.targetSpot] = pieces[move.startingSpot];
            pieces[move.targetSpot].Move(move.targetSpot, simulation);
            pieces[move.startingSpot] = null;
        }

        public void DeletePiece(Spot spot)
        {
            pieces[spot] = null;
        }

        public bool IsCheck(bool checkedColor)
        {
            Spot kingSpot = checkedColor ? whiteKingSpot : blackKingSpot;
            return IsSquareAttacked(kingSpot, !checkedColor);
        }

        public bool IsSquareAttacked(Spot spot, bool attackerColor)
        {
            foreach (Move move in attackerColor ?  whiteMoves : blackMoves)
                if (move.targetSpot == spot) return true;

            return false;
        }

        public void UpdatePosition()
        {
            whiteMoves.Clear();
            blackMoves.Clear();

            pieces.Keys.ToList().ForEach(spot =>
            {
                if (pieces[spot] == null) return;

                //Update king spots
                if (pieces[spot] is King)
                {
                    if (pieces[spot].isWhite) whiteKingSpot = spot;
                    else blackKingSpot = spot;
                }

                //Update possible moves
                List<Move> possiblePieceMoves = pieces[spot].FindPossibleMoves();

                if (pieces[spot].isWhite)
                    whiteMoves.AddRange(possiblePieceMoves);
                else
                    blackMoves.AddRange(possiblePieceMoves);
            });

            //Prevent self check
            for (int i = 0; i < possibleMoves.Count; i++)
            {
                if (ScanMoveForSelfCheck(possibleMoves[i]))
                {
                    possibleMoves.RemoveAt(i);
                    i--;
                }
            }

            //Debug.Log($"Found {possibleMoves.Count} possible moves");
        }

        private bool ScanMoveForSelfCheck(Move move)
        {

            Piece targetPiece = pieces[move.targetSpot];
            bool pieceColor = pieces[move.startingSpot].isWhite;

            Spot kingSpot = pieceColor ? whiteKingSpot : blackKingSpot;
            if (pieces[move.startingSpot] is King) kingSpot = move.targetSpot;

            //Saving en passant spot
            Spot enPassantSpotCopy = this.enPassantSpot;
            Spot enPassantPieceSpot = new Spot(enPassantSpotCopy.rank, enPassantSpotCopy.file == 3 ? 4 : 5);
            Piece enPassantPiece = null;
            bool enPassantSpotValid = enPassantSpotCopy != new Spot(1, 1);

            if (enPassantSpotValid)
                enPassantPiece = pieces[enPassantPieceSpot];

            MakeMoveRaw(move, true);

            bool isCheck = pieces.Where(entry => entry.Value != null && entry.Value.isWhite != pieceColor)
                           .SelectMany(entry => entry.Value.FindPossibleMoves())
                           .Any(move => move.targetSpot == kingSpot);

            MakeMoveRaw(new Move(move.targetSpot, move.startingSpot), true);
            pieces[move.targetSpot] = targetPiece;

            //Loading en passant spot
            if(enPassantSpotValid)
            {
                this.enPassantSpot = enPassantSpotCopy;
                pieces[enPassantPieceSpot] = enPassantPiece;
            }

            return isCheck;
        }
    }

    public enum CastleType
    {
        WHITE_KING,
        WHITE_QUEEN,

        BLACK_KING,
        BLACK_QUEEN
    }
}
