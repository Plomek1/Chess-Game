using UnityEngine;
using UnityEngine.EventSystems;

namespace Chess.Gameplay
{
    [RequireComponent(typeof(GameplayBoard))]
    public class MouseInput : MonoBehaviour
    {
        private GameplaySquare hoveredSquare;
        private GameplaySquare highlightedSquare;
        private GameplaySquare clickedSquare;

        private Transform lastHit;
        private GameplayBoard board;

        private void Start() 
        {
            board = GetComponent<GameplayBoard>();
        }

        private void Update()
        {
            RayForSquare();

            if (Input.GetMouseButtonDown(0))
            {
                board.ClickSquare(hoveredSquare);
                if (hoveredSquare != null) clickedSquare = hoveredSquare;
            }

            if(Input.GetMouseButtonUp(0) && hoveredSquare != clickedSquare) //Check if piece was dragged on different square
                board.ClickSquare(hoveredSquare);
        }

        private void RayForSquare()
        {
            if (IsMouseOverUI()) return;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                if (hitInfo.transform != lastHit)
                {
                    if (hitInfo.transform.TryGetComponent(out GameplaySquare square))
                    {
                        hoveredSquare = square;

                        if(hoveredSquare.squareState == SquareState.SELECTED)
                        {
                            highlightedSquare?.Deselect();
                            highlightedSquare = null;
                        }
                        else HighlightHoveredSquare();
                    }
                    else
                    {
                        highlightedSquare?.Deselect();
                        highlightedSquare = null;
                        hoveredSquare = null;
                        lastHit = null;
                    }

                    lastHit = hitInfo.transform;
                }
            }
        }

        private bool IsMouseOverUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
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
