using UnityEngine;

namespace Chess.Gameplay
{
    [RequireComponent(typeof(GameplayBoard))]
    public class MouseInput : MonoBehaviour
    {
        private GameplaySquare hoveredSquare;
        private GameplaySquare highlightedSquare;

        private Transform lastHit;
        private GameplayBoard board;

        private void Start()
        {
            board = GetComponent<GameplayBoard>();
        }

        private void Update()
        {
            RayForSquare();

            //TODO: PIECE DRAGGING
            if (Input.GetMouseButtonDown(0))
                board.SelectSquare(hoveredSquare);
        }

        private void RayForSquare()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo) && hitInfo.transform.TryGetComponent(out GameplaySquare square))
            {
                if (hitInfo.transform != lastHit)
                {
                    hoveredSquare = square;

                    if(hoveredSquare.squareState == SquareState.SELECTED)
                    {
                        highlightedSquare?.Deselect();
                        highlightedSquare = null;
                    }
                    else HighlightHoveredSquare();

                    lastHit = hitInfo.transform;
                }
            }
            else
            {
                highlightedSquare?.Deselect();
                highlightedSquare = null;
                lastHit = null;
            }
        }

        private void HighlightHoveredSquare()
        {
            if (highlightedSquare && highlightedSquare.squareState == SquareState.HIGHLIGHTED)
                highlightedSquare?.Deselect();

            highlightedSquare = hoveredSquare;
            highlightedSquare.Highlight();
        }
    }
}
