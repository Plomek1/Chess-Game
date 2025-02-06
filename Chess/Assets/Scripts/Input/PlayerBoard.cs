using Chess.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Chess.Gameplay
{
    public class PlayerBoard : MonoBehaviour
    {
        [SerializeField]
        private PlayerBoardSquare squarePrefab;

        private Dictionary<Square, PlayerBoardSquare> squares;

        private void Start()
        {
            SpawnBoard();
        }

        private void SpawnBoard()
        {
            squares = new Dictionary<Square, PlayerBoardSquare>(64);
            float squareSize = squarePrefab.transform.localScale.x;
            bool isWhite = false;

            for (int file = 1; file<= 8; file++)
            {
                for (int rank = 1; rank <= 8; rank++)
                {
                    PlayerBoardSquare square = Instantiate(squarePrefab);
                    square.transform.SetParent(transform);
                    square.Init(new Square(rank, file), isWhite);
                    isWhite = !isWhite;
                }
                isWhite = !isWhite;
            }
        }
    }
}
