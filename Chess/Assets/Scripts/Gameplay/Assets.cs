using UnityEngine;

namespace Chess.Gameplay
{
    public class Assets : MonoBehaviour
    {
        private static Assets _instance;

        public static Assets Instance { get {

                if (_instance == null) _instance = Instantiate((GameObject)Resources.Load("AssetsHolder")).GetComponent<Assets>();
                return _instance; 
            } 
        }

        [Header("Prefabs")]
        public GameplayPiece pref_King; 
        public GameplayPiece pref_Queen; 
        public GameplayPiece pref_Rook; 
        public GameplayPiece pref_Bishop; 
        public GameplayPiece pref_Knight; 
        public GameplayPiece pref_Pawn; 

        [Header("Materials")]
        public Material mat_WhiteSquareIdle;
        public Material mat_WhiteSquareHighlighted;
        public Material mat_WhiteSquareSelected;

        [Space(10)]

        public Material mat_BlackSquareIdle;
        public Material mat_BlackSquareHighlighted;
        public Material mat_BlackSquareSelected;
    }
}
