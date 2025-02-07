using Chess.Core;
using System;
using UnityEngine;

namespace Chess.Gameplay
{
    public class GameplaySquare : MonoBehaviour
    {
        public SquareState squareState {  get; private set; }
        public Spot spot {  get; private set; }
        public bool isWhite { get; private set; }

        private GameplayPiece gameplayPiece;
        private MeshRenderer meshRenderer;

        public void Init(Spot spot, bool isWhite)
        {
            this.spot = spot;
            this.isWhite = isWhite;
            name = spot.notation;

            //Setting position
            float size = transform.localScale.x;
            Vector3 a1Pos = -new Vector3(size * 3.5f, 0, size * 3.5f);
            Vector3 pos = a1Pos + new Vector3(spot.rank - 1, 0, spot.file - 1) * size;
            transform.position = pos;

            meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.material = isWhite ? Assets.Instance.mat_WhiteSquareIdle : Assets.Instance.mat_BlackSquareIdle;
        }

        public void SpawnPiece(Piece piece)
        {
            if (gameplayPiece) Destroy(gameObject.gameObject);
            if (piece == null) return;

            //Find out what type of the piece we need to spawn
            if      (piece is King)   gameplayPiece = Instantiate(Assets.Instance.pref_King);
            else if (piece is Queen)  gameplayPiece = Instantiate(Assets.Instance.pref_Queen);
            else if (piece is Rook)   gameplayPiece = Instantiate(Assets.Instance.pref_Rook);
            else if (piece is Bishop) gameplayPiece = Instantiate(Assets.Instance.pref_Bishop);
            else if (piece is Knight) gameplayPiece = Instantiate(Assets.Instance.pref_Knight);
            else if (piece is Pawn)   gameplayPiece = Instantiate(Assets.Instance.pref_Pawn);

            gameplayPiece.transform.SetParent(transform);
            gameplayPiece.transform.localPosition = new Vector3(0, transform.localScale.y * 5, 0);
            gameplayPiece.SetColor(piece.isWhite);
        }
        
        public void Deselect()
        {
            squareState = SquareState.IDLE;
            meshRenderer.material = isWhite ? Assets.Instance.mat_WhiteSquareIdle : Assets.Instance.mat_BlackSquareIdle;
        }

        public void Highlight()
        {
            squareState = SquareState.HIGHLIGHTED;
            meshRenderer.material = isWhite ? Assets.Instance.mat_WhiteSquareHighlighted : Assets.Instance.mat_BlackSquareHighlighted;
        }

        public void Select()
        {
            if (!gameplayPiece)
            {
                Debug.Log($"Tried to select empty square at: {spot.notation}");
                return;
            }

            squareState = SquareState.SELECTED;
            meshRenderer.material = isWhite ? Assets.Instance.mat_WhiteSquareSelected : Assets.Instance.mat_BlackSquareSelected;
        }
    }

    public enum SquareState
    {
        IDLE,
        HIGHLIGHTED,
        SELECTED
    }
}
