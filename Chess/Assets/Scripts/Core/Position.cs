using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chess.Core
{
    public class Position
    {
        public Dictionary<Spot, Piece> pieces { get; private set; }
        public bool whiteOnMove { get; private set; }
        public List<Move> possibleMoves { get { return whiteOnMove ? whiteMoves : blackMoves; }}
        public Spot whiteKingSpot { get; private set; }
        public Spot blackKingSpot { get; private set; }

        private List<Move> whiteMoves;
        private List<Move> blackMoves;
        
        public bool MakeMove(Move move)
        {
            if (!possibleMoves.Contains(move)) return false;

            MakeMoveRaw(move);

            whiteOnMove = !whiteOnMove;
            UpdatePosition();
            return true;
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

        public void Init(Dictionary<Spot, Piece> pieces, bool whiteOnMove, bool preventSelfCheck = true)
        {
            this.pieces = pieces;
            this.whiteOnMove = whiteOnMove;
            this.whiteMoves = new List<Move>();
            this.blackMoves = new List<Move>();
            UpdatePosition();
        }


        private void UpdatePosition()
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

                //Remove moves resulting in self check
                if (pieces[spot].isWhite == whiteOnMove)
                {
                    for (int i = 0; i < possiblePieceMoves.Count; i++)
                    {
                        if (ScanMoveForSelfCheck(possiblePieceMoves[i]))
                        {
                            Debug.Log(possiblePieceMoves[i]);
                            possiblePieceMoves.RemoveAt(i);
                            i--;
                        }
                    }
                }

                if (pieces[spot].isWhite)
                    whiteMoves.AddRange(possiblePieceMoves);
                else
                    blackMoves.AddRange(possiblePieceMoves);

            });

            //Debug.Log($"Found {possibleMoves.Count} possible moves");
        }

        private bool ScanMoveForSelfCheck(Move move)
        {
            Piece targetPiece = pieces[move.targetSpot];
            bool pieceColor = pieces[move.startingSpot].isWhite;

            Spot kingSpot;
            if (pieces[move.startingSpot] is King) kingSpot = move.targetSpot;
            else kingSpot = pieceColor? whiteKingSpot : blackKingSpot;

            MakeMoveRaw(move);

            bool isCheck = pieces.Where(entry => entry.Value != null && entry.Value.isWhite != pieceColor) //pieces[move.targetSpot] is King is necessary
                           .SelectMany(entry => entry.Value.FindPossibleMoves())                           //for cases where we simulate king moves
                           .Any(move => move.targetSpot == kingSpot);   //because then kingSpot isnt valid

            MakeMoveRaw(new Move(move.targetSpot, move.startingSpot));
            pieces[move.targetSpot] = targetPiece;

            return isCheck;
        }

        private void MakeMoveRaw(Move move)
        {

            pieces[move.targetSpot] = pieces[move.startingSpot];
            pieces[move.targetSpot].Move(move.targetSpot);
            pieces[move.startingSpot] = null;
        }
    }
}
