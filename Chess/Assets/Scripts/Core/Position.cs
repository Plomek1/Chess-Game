using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chess.Core
{
    public class Position
    {
        public Dictionary<Spot, Piece> pieces { get; private set; }
        public List<Move> possibleMoves { get; private set; }
        public bool whiteOnMove { get; private set; }
        public Spot whiteKingSpot { get; private set; }
        public Spot blackKingSpot { get; private set; }

        public bool MakeMove(Move move)
        {
            if (!possibleMoves.Contains(move)) return false;

            pieces[move.targetSpot] = pieces[move.startingSpot];
            pieces[move.targetSpot].Move(move.targetSpot);
            pieces[move.startingSpot] = null;
            whiteOnMove = !whiteOnMove;

            UpdatePosition();
            return true;
        }

        public void UpdatePosition()
        {
            possibleMoves.Clear();

            pieces.Keys.ToList().ForEach(spot =>
            {
                if (pieces[spot] == null) return;

                //Update king spots
                if (pieces[spot] is King)
                {
                    if (pieces[spot].isWhite) whiteKingSpot = spot;
                    else blackKingSpot = spot;
                }

                //Find possible moves
                if (pieces[spot].isWhite == whiteOnMove)
                    possibleMoves.AddRange(pieces[spot].FindPossibleMoves());
            });

            //Debug.Log($"Found {possibleMoves.Count} possible moves");
        }

        public bool IsCheck(bool checkedColor)
        {
            Spot kingSpot = checkedColor ? whiteKingSpot : blackKingSpot;
            return IsSquareAttacked(kingSpot, !checkedColor);
        }

        public bool IsSquareAttacked(Spot spot, bool attackerColor)
        {
            if (attackerColor == whiteOnMove)
            {
                foreach (Move move in possibleMoves)
                    if (move.targetSpot == spot) return true;
            }
            else
            {
                List<Move> possibleNextMoves = new List<Move>();
                pieces.Keys.ToList().ForEach(attackerSpot => {
                    if (pieces[attackerSpot] == null) return;
                    if (pieces[attackerSpot].isWhite == attackerColor)
                        possibleNextMoves.AddRange(pieces[attackerSpot].FindPossibleMoves());
                });

                foreach (Move move in possibleNextMoves)
                    if (move.targetSpot == spot) return true;
            }

            return false;
        }

        public void Init(Dictionary<Spot, Piece> pieces, bool whiteOnMove)
        {
            this.pieces = pieces;
            this.whiteOnMove = whiteOnMove;
            this.possibleMoves = new List<Move>();
            UpdatePosition();
        }
    }
}
