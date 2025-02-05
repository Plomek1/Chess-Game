using Chess.Core;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Board board;

    private void Start()
    {
        board = new Board();
    }
}
