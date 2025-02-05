using Unity.Collections;
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
            if(NotationParser.CoordinatesToNotation(file, rank, out string notation))
            {
                this.file = file;
                this.rank = rank;
                this.notation = notation;
                return;
            }

            this.file = 1;
            this.rank = 1;
            this.notation = "a1";
            Debug.LogWarning($"Created spot with invalid coordinates: ({file}, {rank})");
        }

        public Spot(string notation)
        {
            if(NotationParser.NotationToCoordinates(notation, out int file, out int rank))
            {
                this.file = file;
                this.rank = rank;
                this.notation = notation;
                return;
            }

            this.file = 1;
            this.rank = 1;
            this.notation = "a1";
            Debug.LogWarning($"Created spot with invalid notation: {notation}");
        }

        public override string ToString() { return notation; }
    }
}
