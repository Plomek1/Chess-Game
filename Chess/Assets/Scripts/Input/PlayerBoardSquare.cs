using Chess.Core;
using System;
using UnityEngine;

namespace Chess.Gameplay
{
    public class PlayerBoardSquare : MonoBehaviour
    {
        Action<Spot> SquareSelected;

        [SerializeField]
        private Material whiteMat;
        [SerializeField]
        private Material blackMat;

        private Spot spot;

        public void Init(Spot spot, bool isWhite)
        {
            this.spot = spot;
            name = spot.notation;

            //Setting position
            float size = transform.localScale.x;
            Vector3 a1Pos = -new Vector3(size * 3.5f, 0, size * 3.5f);
            Vector3 pos = a1Pos + new Vector3(spot.file - 1, 0, spot.rank - 1) * size;
            transform.position = pos;

            GetComponent<MeshRenderer>().material = isWhite? whiteMat : blackMat;
        }
    }
}
