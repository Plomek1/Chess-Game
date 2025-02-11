using Chess.Core;
using System;
using UnityEngine;

namespace Chess.Gameplay.UI
{
    public class PromotionMenu : MonoBehaviour
    {
        [SerializeField] private GameplayBoard board;
        [SerializeField] private RectTransform menuRoot;

        private void Start()
        {
            board.PromptPromotion += Open;
        }

        public void PromoteQueen()
        {
            board.PromotePiece(typeof(Queen));
            Close();
        }

        public void PromoteRook()
        {
            board.PromotePiece(typeof(Rook));
            Close();
        }

        public void PromoteBishop()
        {
            board.PromotePiece(typeof(Bishop));
            Close();
        }

        public void PromoteKnight()
        {
            board.PromotePiece(typeof(Knight));
            Close();
        }

        private void Open()
        {
            menuRoot.gameObject.SetActive(true);
        }

        private void Close()
        {
            menuRoot.gameObject.SetActive(false);
        }
    }
}
