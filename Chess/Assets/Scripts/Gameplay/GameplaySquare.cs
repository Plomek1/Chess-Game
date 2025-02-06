using Chess.Core;
using System;
using UnityEngine;

namespace Chess.Gameplay
{
    public class GameplaySquare : MonoBehaviour
    {
        Action<Square> SquareSelected;

        private Square square;
        private bool isWhite;

        private GameplayPiece gameplayPiece;
        private MeshRenderer meshRenderer;

        public void Init(Square square, bool isWhite)
        {
            this.square = square;
            this.isWhite = isWhite;
            name = square.notation;

            //Setting position
            float size = transform.localScale.x;
            Vector3 a1Pos = -new Vector3(size * 3.5f, 0, size * 3.5f);
            Vector3 pos = a1Pos + new Vector3(square.rank - 1, 0, square.file - 1) * size;
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
            meshRenderer.material = isWhite ? Assets.Instance.mat_WhiteSquareIdle : Assets.Instance.mat_BlackSquareIdle;
        }

        public void Highlight()
        {
            meshRenderer.material = isWhite ? Assets.Instance.mat_WhiteSquareHighlighted : Assets.Instance.mat_BlackSquareHighlighted;
        }

        public void Select()
        {
            meshRenderer.material = isWhite ? Assets.Instance.mat_WhiteSquareSelected : Assets.Instance.mat_BlackSquareSelected;
        }
    }
}
