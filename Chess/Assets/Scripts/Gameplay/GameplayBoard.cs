using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Chess.Core;

namespace Chess.Gameplay
{
    public class GameplayBoard : MonoBehaviour
    {
        public Action PromptPromotion;

        [TextArea(1, 2)] [SerializeField] private string fenToLoad;
        [SerializeField] private GameplaySquare squarePrefab;

        private const string FEN_DEFAULT = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        
        private Board board;
        private Dictionary<Spot, GameplaySquare> squares;
        private List<GameplaySquare> markedSquares;

        private GameplaySquare selectedSquare;
        private Spot promotionSquare;

        private void Start()
        {
            squares = new Dictionary<Spot, GameplaySquare>(64);
            markedSquares = new List<GameplaySquare>();
            SpawnBoard();
        }

        public void ClickSquare(GameplaySquare square)
        {
            if (!square)
            {
                DeselectSquare();
                return;
            }

            Piece targetSquarePiece = board.GetPiece(square.spot);

            if (selectedSquare)
            {
                //Make move
                if (markedSquares.Contains(square))
                {
                    board.MakeMove(new Move(selectedSquare.spot, square.spot));
                    DeselectSquare();
                    return;
                }
                
                //Deselect if clicked again on the same square
                if(square == selectedSquare)
                {
                    DeselectSquare();
                    return;
                }

                //Reselect different square
                if (targetSquarePiece != null && targetSquarePiece.isWhite == board.IsWhiteOnMove())
                {
                    DeselectSquare();
                    SelectSquare(square);
                    return;
                }
            }

            //Select square
            if (targetSquarePiece != null && targetSquarePiece.isWhite == board.IsWhiteOnMove())
            {
                SelectSquare(square);   
                return;
            }

            DeselectSquare();
            return;
        }

        public void PromotePiece(Type pieceType)
        {
            board.PromotePiece(pieceType);
            squares[promotionSquare].DeletePiece();
            squares[promotionSquare].SpawnPiece(board.GetPiece(promotionSquare));
        }

        private void PromptPromotionDelegate(Spot spot)
        {
            promotionSquare = spot;
            PromptPromotion?.Invoke();
        }

        private void SelectSquare(GameplaySquare square)
        {
            selectedSquare = square;
            selectedSquare.Select();
            MarkMoves(board.GetPiece(square.spot));
        }

        private void DeselectSquare()
        {
            if (selectedSquare == null) return;
            selectedSquare.Deselect();
            selectedSquare = null;
            UnmarkMoves();
        }

        private void MarkMoves(Piece selectedPiece)
        {
            foreach (Move move in board.GetPossibleMoves())
            {
                if (move.startingSpot == selectedPiece.spot)
                {
                    GameplaySquare markedSquare = squares[move.targetSpot];
                    markedSquare.EnableMoveMarker();
                    markedSquares.Add(markedSquare);
                }
            }
        }

        private void UnmarkMoves()
        {
            markedSquares.ForEach(square  => square.DisableMoveMarker());
            markedSquares.Clear();
        }
        private void LoadPosition()
        {
            var pieces = board.GetAllPieces();
            pieces.Keys.ToList().ForEach(key => squares[key].SpawnPiece(pieces[key]));
        }

        private void MovePiece(Move move)
        {
            GameplayPiece piece = squares[move.startingSpot].GivePiece();
            squares[move.targetSpot].GetPiece(piece);
        }

        private void DeletePiece(Spot spot)
        {
            squares[spot].DeletePiece();
        }

        private void SpawnBoard()
        {
            board = new Board(PromptPromotionDelegate);
            board.PositionSet += LoadPosition;
            board.PieceMoved += MovePiece;
            board.PieceDeleted += DeletePiece;

            float squareSize = squarePrefab.transform.localScale.x;
            bool isWhite = false;

            for (int file = 1; file <= 8; file++)
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
    }
}
