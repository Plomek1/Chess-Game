using UnityEngine;


namespace Chess.Core
{
    public class Board : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Spot spot1 = new Spot(3, 2);
            Spot spot2 = new Spot("h6");
            Spot spot3 = new Spot("1");

            Debug.Log(spot1);
            Debug.Log(spot2);
            Debug.Log(spot3);
        }
    }
}
