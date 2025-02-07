using UnityEngine;

namespace Chess.Gameplay
{
    public class GameplayPiece : MonoBehaviour
    {
        [SerializeField] private Material whiteMaterial;
        [SerializeField] private Material blackMaterial;

        public void SetColor(bool isWhite)
        {
            GetComponentInChildren<MeshRenderer>().material = isWhite ? whiteMaterial : blackMaterial;
        }

        public void Delete()
        {
            Destroy(gameObject);
        }
    }
}
