using UnityEngine;

namespace Chess.Gameplay
{
    public class MouseInput : MonoBehaviour
    {
        private PlayerBoardSquare highlightedSquare;
        private PlayerBoardSquare selectedSquare;
        private PlayerBoardSquare hoveredSquare;

        private Transform lastHit;


        private void Update()
        {
            RayForSquare();

            //TODO: PIECE DRAGGING
            if (Input.GetMouseButtonDown(0))
            {
                if(selectedSquare)
                {
                    if (highlightedSquare)
                    {
                        Debug.Log("move");
                    }
                    Debug.Log("aha");
                    DeselectSquare();
                }

                else SelectHighlightedSquare();
            }
        }

        private void HighlightHoveredSquare()
        {
            highlightedSquare?.Deselect();
            highlightedSquare = hoveredSquare;
            highlightedSquare.Highlight();
        }

        private void SelectHighlightedSquare()
        {
            selectedSquare?.Deselect();
            highlightedSquare?.Select();

            selectedSquare = highlightedSquare;
            highlightedSquare = null;
        }

        private void DeselectSquare()
        {
            if (!selectedSquare) return;

            if (hoveredSquare == selectedSquare) selectedSquare.Highlight();
            else selectedSquare.Deselect();

            selectedSquare = null;
        }

        private void RayForSquare()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo) && hitInfo.transform.TryGetComponent(out PlayerBoardSquare square))
            {
                if (hitInfo.transform != lastHit)
                {
                    hoveredSquare = square;
                    if(hoveredSquare == selectedSquare)
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
    }
}
