namespace Chess.Core
{
    public struct Move    
    {
        public Spot startingSpot;
        public Spot targetSpot;

        public Move(Spot startingSpot, Spot targetSpot)
        {
            this.startingSpot = startingSpot;
            this.targetSpot = targetSpot;
        }
    }
}
