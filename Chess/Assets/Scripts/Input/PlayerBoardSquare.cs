using Chess.Core;
using System;
using UnityEngine;

namespace Chess.Gameplay
{
    public class PlayerBoardSquare : MonoBehaviour
    {
        Action<Square> SquareSelected;

        [SerializeField]
        private Material matIdleWhite;
        [SerializeField]
        private Material matIdleBlack;

        [SerializeField]
        private Material matHighlightedWhite;
        [SerializeField]
        private Material matHighlightedBlack;

        [SerializeField]
        private Material matSelectedWhite;
        [SerializeField]
        private Material matSelectedBlack;

        private Square square;
        private bool isWhite;

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
            meshRenderer.material = isWhite ? matIdleWhite : matIdleBlack;
        }

        public void Highlight()
        {
            meshRenderer.material = isWhite ? matHighlightedWhite : matHighlightedBlack;
            Debug.Log(square.notation);
        }
        public void Select()
        {
            meshRenderer.material = isWhite ? matSelectedWhite : matSelectedBlack;

        }
        public void Deselect()
        {
            meshRenderer.material = isWhite ? matIdleWhite : matIdleBlack;
        }
    }
}
