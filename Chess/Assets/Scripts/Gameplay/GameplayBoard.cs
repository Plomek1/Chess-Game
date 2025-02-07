using Chess.Core;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

namespace Chess.Gameplay
{
    public class GameplayBoard : MonoBehaviour
    {
        [TextArea(1, 2)] [SerializeField] private string fenToLoad;
        [SerializeField] private GameplaySquare squarePrefab;

        private const string FEN_DEFAULT = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        
        private Board board;
        private Dictionary<Spot, GameplaySquare> squares;

        private GameplaySquare selectedSquare;

        private void Start()
        {
            SpawnBoard();
        }

        private void SpawnBoard()
        {
            board = new Board();
            board.PositionSet += LoadPosition;

            squares = new Dictionary<Spot, GameplaySquare>(64);
            float squareSize = squarePrefab.transform.localScale.x;
            bool isWhite = false;

            for (int file = 1; file<= 8; file++)
            {
                for (int rank = 1; rank <= 8; rank++)
                {
                    GameplaySquare square = Instantiate(squarePrefab);
                    Spot squarePos = new Spot(rank, file);
                    square.transform.SetParent(transform);
                    square.Init(squarePos, isWhite);
                    isWhite = !isWhite;

                    squares.Add(squarePos, square);
                }
                isWhite = !isWhite;
            }

            board.LoadPositionFromFEN(fenToLoad);
        }

        public void SelectSquare(GameplaySquare square)
        {
            Piece targetSquarePiece = board.pieces[square.spot];

            if (selectedSquare)
            {
                if (targetSquarePiece == null) //Move
                {
                    Debug.Log("Move");
                    DeselectSquare();
                    return;
                }
                
                if (targetSquarePiece.isWhite != board.whiteOnMove) //Take
                {
                    Debug.Log("Take");
                    DeselectSquare();
                    return;
                }

                //Reselect different square
                DeselectSquare();
                selectedSquare = square;
                selectedSquare.Select();
                return;
            }

            if (targetSquarePiece != null && targetSquarePiece.isWhite == board.whiteOnMove)
            {
                selectedSquare = square;
                selectedSquare.Select();
                return;
            }

            DeselectSquare();
            return;
        }

        private void DeselectSquare()
        {
            if (selectedSquare == null) return;
            selectedSquare.Deselect();
            selectedSquare = null;
        }

        private void LoadPosition()
        {
            board.pieces.Keys.ToList().ForEach(key => squares[key].SpawnPiece(board.pieces[key]));
        }
    }
}
