using UnityEngine;

namespace TechJuego.FruitSliceMerge
{
    // Class to manage game-related events using delegates
    public class GameEvents
    {
        public delegate void OnAction();
        public static OnAction OnGameEnd;
        public static OnAction OnMosueUp;
        public static OnAction OnGameStart;
        public static OnAction OnShowRateUs;
        public static OnAction OnUpdateScore;
        public static OnAction OnUpdateBombCount;
        public delegate void OnAction1(Vector3 pos);
        public static OnAction1 OnMosueDown;
        public delegate void OnAction2(Booster booster);
        public static OnAction2 OnSelectBooster;
        public static OnAction2 OnGetBooster;
        public static OnAction2 OnUseBooster;
    }
}

