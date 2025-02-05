using UnityEngine;

namespace Chess.Core
{
    public static class NotationParser
    {
        public static bool NotationToCoordinates(string notation, out int file, out int rank)
        {
            if (notation.Length != 2)
            {
                file = 1;
                rank = 1;
                return false;
            }
            
            int fileLocal = notation[0] % 32;
            int rankLocal = notation[1] - '0';

            if (ValidateCoordinates(fileLocal, rankLocal))
            {
                file = fileLocal;
                rank = rankLocal;
                return true;
            }

            file = 1;
            rank = 1;
            return false;
        }

        public static bool CoordinatesToNotation(int file, int rank, out string notation)
        {
            if (ValidateCoordinates(file, rank))
            {
                char fileChar = (char)('a' + (file - 1));
                notation = fileChar + rank.ToString();
                return true;
            }

            notation = "a1";
            return false;
        }

        public static bool ValidateCoordinates(int file, int rank) => file >= 1 && file <= 8 && rank >= 1 && rank <= 8;
    }
}

