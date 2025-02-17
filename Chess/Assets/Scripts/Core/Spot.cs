using System;
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

        public static bool operator ==( Spot left, Spot right ) => left.rank == right.rank && left.file == right.file;
        public static bool operator !=(Spot left, Spot right) => !(left == right);
        public override bool Equals(object obj) => obj is Spot other && this == (Spot)other;
        public override int GetHashCode() => HashCode.Combine(rank, file);
        public override string ToString() => notation;
    }
}
