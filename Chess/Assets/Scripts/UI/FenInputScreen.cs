using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Chess.Gameplay.UI
{
    public class FenInputScreen : MonoBehaviour
    {
        private const string regPattern = @"^([rnbqkpRNBQKP1-8]+/){7}[rnbqkpRNBQKP1-8]+ [wb] (K?Q?k?q?|-) ([a-h][36]|-) (\d+) (\d+)$";

        [SerializeField] private TMP_InputField fenInput;

        public void Load()
        {
            fenInput.text = fenInput.text.Trim(' ', '\n');
            if(Regex.IsMatch(fenInput.text, regPattern))
            {
                GameplayBoard.fenInput = fenInput.text;
                SceneManager.LoadScene(1);
            }
        }

        public void Close()
        {
            fenInput.text = "";
            gameObject.SetActive(false);
        }

    }
}
