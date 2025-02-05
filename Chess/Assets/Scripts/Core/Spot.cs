using UnityEngine;

namespace Chess.Core
{
    public struct Spot
    {
        public int rank { get; private set; }
        public int file { get; private set; }
        public string notation { get; private set; }

        public Spot(int file, int rank)
        {
            this.file = file;
            this.rank = rank;

            char fileChar = (char)('a' + (file - 1));
            notation = fileChar + rank.ToString();

            if(!Validate())
            {
                Debug.LogWarning($"Created spot with invalid coordinates: ({file}, {rank})");
                this.file = 1;
                this.rank = 1;
                notation = "a1";
            }
        }

        public Spot(string notation)
        {
            if(notation.Length != 2)
            {
                Debug.LogWarning($"Created spot with invalid notation: {notation}");
                this.file = 1;
                this.rank = 1;
                this.notation = "a1";
                return;
            }

            char fileChar = notation[0];
            char rankChar = notation[1];
            this.file = fileChar % 32;
            this.rank = rankChar - '0';

            this.notation = notation;

            if (!Validate())
            {
                Debug.LogWarning($"Created spot with invalid notation: {notation}");
                this.file = 1;
                this.rank = 1;
                this.notation = "a1";
            }
        }

        public override string ToString()
        {
            return notation;
        }

        private bool Validate()
        {
            return file >= 1 && file <= 8 && rank >= 1 && rank <= 8;
        }
    }
}
