using Chess.Core;
using System;
using UnityEngine;

namespace Chess.Gameplay
{
    public class PlayerBoardSquare : MonoBehaviour
    {
        Action<Spot> SquareSelected;

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

        private Spot spot;
        private bool isWhite;

        private MeshRenderer meshRenderer;

        public void Init(Spot spot, bool isWhite)
        {
            this.spot = spot;
            this.isWhite = isWhite;
            name = spot.notation;

            //Setting position
            float size = transform.localScale.x;
            Vector3 a1Pos = -new Vector3(size * 3.5f, 0, size * 3.5f);
            Vector3 pos = a1Pos + new Vector3(spot.file - 1, 0, spot.rank - 1) * size;
            transform.position = pos;

            meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.material = isWhite ? matIdleWhite : matIdleBlack;
        }

        public void Highlight()
        {
            meshRenderer.material = isWhite ? matHighlightedWhite : matHighlightedBlack;
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
