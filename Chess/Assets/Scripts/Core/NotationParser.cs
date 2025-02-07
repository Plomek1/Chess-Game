using System;
using System.Collections.Generic;
using UnityEngine;
namespace Chess.Core
{
    public static class NotationParser
    {
        public static bool NotationToCoordinates(string notation, out int rank, out int file)
        {
            if (notation.Length != 2)
            {
                rank = 1;
                file = 1;
                return false;
            }
            
            int rankLocal = notation[0] % 32;
            int fileLocal = notation[1] - '0';

            if (ValidateCoordinates(rankLocal, fileLocal))
            {
                rank = rankLocal;
                file = fileLocal;
                return true;
            }

            rank = 1;
            file = 1;
            return false;
        }

        public static bool CoordinatesToNotation(int rank, int file, out string notation)
        {
            if (ValidateCoordinates(rank, file))
            {
                char rankChar = (char)('a' + (rank - 1));
                notation = rankChar + file.ToString();
                return true;
            }

            notation = "a1";
            return false;
        }

        public static Piece NotationToPiece(char pieceChar)
        {
            pieceChar = char.ToUpper(pieceChar);
            if (pieceTypes.TryGetValue(pieceChar, out Type pieceType))
                return (Piece)Activator.CreateInstance(pieceType);
            return null;
        }

        public static bool ValidateCoordinates(int rank, int file) => rank >= 1 && rank <= 8 && file >= 1 && file <= 8;

        private static Dictionary<char, Type> pieceTypes = new Dictionary<char, Type> 
        {
            { 'K', typeof(King) },
            { 'Q', typeof(Queen) },
            { 'R', typeof(Rook) },
            { 'B', typeof(Bishop) },
            { 'N', typeof(Knight) },
            { 'P', typeof(Pawn) },
        };
            

    }
}

