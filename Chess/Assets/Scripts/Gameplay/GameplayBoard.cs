using Chess.Core;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chess.Gameplay
{
    public class GameplayBoard : MonoBehaviour
    {
        [SerializeField] private string fenToLoad;
        [SerializeField] private GameplaySquare squarePrefab;
        
        private const string FEN_DEFAULT = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        
        private Board board;
        private Dictionary<Square, GameplaySquare> squares;

        private void Start()
        {
            SpawnBoard();
        }

        private void SpawnBoard()
        {
            board = new Board();
            board.PositionSet += SetPosition;

            squares = new Dictionary<Square, GameplaySquare>(64);
            float squareSize = squarePrefab.transform.localScale.x;
            bool isWhite = false;

            for (int file = 1; file<= 8; file++)
            {
                for (int rank = 1; rank <= 8; rank++)
                {
                    GameplaySquare square = Instantiate(squarePrefab);
                    Square squarePos = new Square(rank, file);
                    square.transform.SetParent(transform);
                    square.Init(squarePos, isWhite);
                    isWhite = !isWhite;

                    squares.Add(squarePos, square);
                }
                isWhite = !isWhite;
            }

            board.LoadPositionFromFEN(fenToLoad);
        }

        private void SetPosition()
        {
            board.pieces.Keys.ToList().ForEach(key => squares[key].SpawnPiece(board.pieces[key]));
        }
    }
}
