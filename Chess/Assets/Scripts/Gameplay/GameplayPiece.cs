using DG.Tweening;
using UnityEngine;

namespace Chess.Gameplay
{
    public class GameplayPiece : MonoBehaviour
    {
        [SerializeField] private float deleteDuration;
        [SerializeField] private Material whiteMaterial;
        [SerializeField] private Material blackMaterial;

        public void SetColor(bool isWhite)
        {
            GetComponentInChildren<MeshRenderer>().material = isWhite ? whiteMaterial : blackMaterial;
        }

        public void Delete()
        {
            DOTween.Kill(transform);
            transform.DOScale(Vector3.zero, deleteDuration).OnComplete(() => Destroy(gameObject));
        }
    }
}
