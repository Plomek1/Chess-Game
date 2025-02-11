using DG.Tweening;
using UnityEngine;

namespace Chess.Gameplay
{
    public class GameplayPiece : MonoBehaviour
    {
        [SerializeField] private float deleteDuration;
        [SerializeField] private Material whiteMaterial;
        [SerializeField] private Material blackMaterial;
        [SerializeField] private float wobbleFactor = 500;
        
        private Transform modelTransform;
        private Vector3 lastPosition;

        private void Start()
        {
            modelTransform = transform.GetChild(0);
        }

        private void Update()
        {
            //Wobble
            Vector3 positionDelta = transform.position - lastPosition;

            Vector3 rotation = new Vector3(-positionDelta.z, 0, positionDelta.x) * wobbleFactor;
            modelTransform.rotation = Quaternion.Euler(rotation);

            lastPosition = transform.position;
        }

        public void SetColor(bool isWhite)
        {
            GetComponentInChildren<MeshRenderer>().material = isWhite ? whiteMaterial : blackMaterial;
        }

        public void Delete()
        {
            DOTween.Kill(transform);
            transform.DOScale(Vector3.zero, deleteDuration).OnComplete(() => Destroy(gameObject, 1f));
        }
    }
}
