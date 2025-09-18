using System.Collections;
using System.Collections.Generic;
using TechJuego.PlanetMerge.Monetization;
using TechJuego.PlanetMerge.Rateus;
using UnityEngine;
namespace TechJuego.PlanetMerge
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
                    case GameState.GameOver:
                        GameEvents.OnGameEnd?.Invoke();
                        break;
                    case GameState.InGameSetting:
                        break;
                    case GameState.InProgress:
                        break;
                    case GameState.PrivacyPolicy:
                        break;
                }
                Debug.Log(">>>>>>>>>>>>>>>>>");
                AdsHandler.Instance.ShowAds(value);
#if UNITY_ANDROID || UNITY_IPHONE
                RateusHandler.Instance.ShowRateus(value);
#endif
            }
        }
    }
}