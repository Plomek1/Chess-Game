using Chess.Core;
using DG.Tweening;
using System;
using UnityEngine;

namespace Chess.Gameplay
{
    public class GameplaySquare : MonoBehaviour
    {
        public SquareState squareState {  get; private set; }
        public Spot spot {  get; private set; }
        public bool isWhite { get; private set; }

        [SerializeField] private GameObject moveMarker;
        [SerializeField] private float highlightedPieceY;
        [SerializeField] private float selectedPieceY;
        [SerializeField] private float baseTweenSpeed = .1f;
        [SerializeField] private float pieceMoveSpeed = 2f;

        private GameplayPiece gameplayPiece;
        private MeshRenderer meshRenderer;

        private Vector3 pieceIdlePos;
        private Vector3 pieceHighlightedPos;
        private Vector3 pieceSelectedPos;
        private bool unbreakableTweenActive;

        public void Init(Spot spot, bool isWhite)
        {
            this.spot = spot;
            this.isWhite = isWhite;
            this.name = spot.notation;

            pieceIdlePos        = new Vector3(0, transform.localScale.y * 5, 0);
            pieceHighlightedPos = pieceIdlePos + new Vector3(0, highlightedPieceY, 0);
            pieceSelectedPos    = pieceIdlePos + new Vector3(0, selectedPieceY, 0);

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
            gameplayPiece.transform.localPosition = pieceIdlePos;
            gameplayPiece.SetColor(piece.isWhite);
        }

        public void GetPiece(GameplayPiece piece)
        {
            if (gameplayPiece) gameplayPiece.Delete();
            gameplayPiece = piece;
            gameplayPiece.transform.SetParent(transform);

            //Moving piece to square
            Sequence sequence = DOTween.Sequence();

            unbreakableTweenActive = true;
            float moveSpeed = baseTweenSpeed / pieceMoveSpeed * piece.transform.localPosition.magnitude;
            sequence.Append(gameplayPiece.transform.DOLocalMove(pieceSelectedPos, moveSpeed));
            sequence.Append(gameplayPiece.transform.DOLocalMove(pieceIdlePos, baseTweenSpeed));
            
            sequence.OnComplete(() =>
            {
                unbreakableTweenActive = false;
                if (squareState == SquareState.HIGHLIGHTED) TweenPiece(pieceHighlightedPos, baseTweenSpeed);
            });

        }

        public GameplayPiece GivePiece()
        {
            GameplayPiece piece = gameplayPiece;
            gameplayPiece = null;
            return piece;
        }
        
        public void Deselect()
        {
            squareState = SquareState.IDLE;
            meshRenderer.material = isWhite ? Assets.Instance.mat_WhiteSquareIdle : Assets.Instance.mat_BlackSquareIdle;        
            TweenPiece(pieceIdlePos, baseTweenSpeed);
        }

        public void Highlight()
        {
            squareState = SquareState.HIGHLIGHTED;
            meshRenderer.material = isWhite ? Assets.Instance.mat_WhiteSquareHighlighted : Assets.Instance.mat_BlackSquareHighlighted;
            TweenPiece(pieceHighlightedPos, baseTweenSpeed);
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
            TweenPiece(pieceSelectedPos, baseTweenSpeed);
        }

        public void EnableMoveMarker() => moveMarker.SetActive(true);
        public void DisableMoveMarker() => moveMarker.SetActive(false);
    
        private void TweenPiece(Vector3 targetPos, float duration, bool unbreakable = false)
        {
            if (unbreakableTweenActive || !gameplayPiece) return;

            unbreakableTweenActive = unbreakable;
            gameplayPiece.transform.DOLocalMove(targetPos, duration).OnComplete(() => unbreakableTweenActive = false);
        }
    }

    public enum SquareState
    {
        IDLE,
        HIGHLIGHTED,
        SELECTED
    }
}
