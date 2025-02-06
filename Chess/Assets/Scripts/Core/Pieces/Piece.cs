namespace Chess.Core
{
    public class Piece
    {
        public bool isWhite {  get; private set; }

        private Board board;

        private Square square;

        public void Init(Board board, bool isWhite, Square square)
        {
            this.board = board;
            this.isWhite = isWhite;
            this.square = square;
        }
    }
}
