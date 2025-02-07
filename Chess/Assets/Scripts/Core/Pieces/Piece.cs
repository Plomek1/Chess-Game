namespace Chess.Core
{
    public abstract class Piece
    {
        public bool isWhite {  get; private set; }

        private Board board;

        private Spot spot;

        public void Init(Board board, bool isWhite, Spot spot)
        {
            this.board = board;
            this.isWhite = isWhite;
            this.spot = spot;
        }
    }
}
