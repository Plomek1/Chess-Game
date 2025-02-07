using Unity.Collections;
using UnityEngine;

namespace Chess.Core
{
    public struct Spot
    {
        public int rank { get; private set; }
        public int file { get; private set; }
        public string notation { get; private set; }

        public Spot(int index)
        {
            int rank = (index-1) % 8 + 1;
            int file = (index-1) / 8 + 1;

            if (NotationParser.CoordinatesToNotation(rank, file, out string notation))
            {
                this.rank = rank;
                this.file = file;
                this.notation = notation;
                return;
            }

            this.rank = 1;
            this.file = 1;
            this.notation = "a1";
            Debug.LogWarning($"Created spot with invalid index: {index}");
        }

        public Spot(int rank, int file)
        {
            if(NotationParser.CoordinatesToNotation(rank, file, out string notation))
            {
                this.rank = rank;
                this.file = file;
                this.notation = notation;
                return;
            }

            this.rank = 1;
            this.file = 1;
            this.notation = "a1";
            Debug.LogWarning($"Created spot with invalid coordinates: ({rank}, {file})");
        }

        public Spot(string notation)
        {
            if(NotationParser.NotationToCoordinates(notation, out int rank, out int file))
            {
                this.rank = rank;
                this.file = file;
                this.notation = notation;
                return;
            }

            this.rank = 1;
            this.file = 1;
            this.notation = "a1";
            Debug.LogWarning($"Created spot with invalid notation: {notation}");
        }

        public override string ToString() { return notation; }
    }
}
