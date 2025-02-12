using UnityEngine;
using Chess.Core;
using TMPro;

namespace Chess.Gameplay.UI
{
    public class GameOverMenu : MonoBehaviour
    {
        [SerializeField] private GameplayBoard board;
        [SerializeField] private RectTransform rootTransform;

        [SerializeField] private TextMeshProUGUI winnerText;
        [SerializeField] private TextMeshProUGUI conditionText;


        private void Start()
        {
            board.Win += Win;
            board.Draw += Draw;
        }

        public void Restart()
        {
            board.ReloadGame();
            rootTransform.gameObject.SetActive(false);
        }

        public void MainMenu()
        {
            Debug.Log("MAIN MENU OPEN");
            rootTransform.gameObject.SetActive(false);
        }

        private void Win(bool winner, GameEndCondition condition)
        {
            rootTransform.gameObject.SetActive(true);
            string winnerString = winner ? "White" : "Black";
            winnerText.text = $"{winnerString} wins!";
            conditionText.text = "By checkmate";
        }

        private void Draw(GameEndCondition condition)
        {
            rootTransform.gameObject.SetActive(true);
            winnerText.text = "Draw!";
            conditionText.text = condition == GameEndCondition.Stalemate ? "By stalemate" : "By 50 move rule";
        }

    }
}
