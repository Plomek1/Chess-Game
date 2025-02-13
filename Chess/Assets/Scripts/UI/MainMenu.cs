using UnityEngine;
using UnityEngine.SceneManagement;

namespace Chess.Gameplay.UI
{
    public class MainMenu : MonoBehaviour
    {
        private const string FEN_DEFAULT = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

        [SerializeField] private RectTransform fenInputScreen;


        public void LoadDefault()
        {
            GameplayBoard.fenInput = FEN_DEFAULT;
            SceneManager.LoadScene(1);
        }

        public void OpenFENInput()
        {
            fenInputScreen.gameObject.SetActive(true);
        }

        public void Exit()
        {
            Debug.Log("QUITTING");
            Application.Quit();
        }
    }
}
