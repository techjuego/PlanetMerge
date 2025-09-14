using System.Collections;
using System.Collections.Generic;
using TechJuego.FruitSliceMerge.Monetization;
using TechJuego.FruitSliceMerge.Rateus;
using UnityEngine;
namespace TechJuego.FruitSliceMerge
{
    public class GameStateHandler : Singleton<GameStateHandler>
    {
        protected GameStateHandler() { }
        private GameState GameState;
        public GameState m_GameState
        {
            get { return GameState; }
            set
            {
                GameState = value;
                switch (value)
                {
                    case GameState.None:
                        break;
                    case GameState.Gameplay:
                        break;
                    case GameState.GameOver:
                        GameEvents.OnGameEnd?.Invoke();
                        break;
                    case GameState.InGameSetting:
                        break;
                }
                AdsHandler.Instance.ShowAds(value);
#if UNITY_ANDROID || UNITY_IPHONE
                RateusHandler.Instance.ShowRateus(value);
#endif
            }
        }
    }
}