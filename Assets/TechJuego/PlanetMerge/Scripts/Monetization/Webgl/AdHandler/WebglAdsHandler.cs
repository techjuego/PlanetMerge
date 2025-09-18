using System;
using System.Collections.Generic;
using UnityEngine;

namespace TechJuego.PlanetMerge.Monetization
{
    public class WebglAdsHandler : MonoBehaviour
    {
        private IAdGetDetail m_AdGetDetails;
        private void Awake()
        {
#if GAMEDISTRIBUTION
            m_AdGetDetails = gameObject.AddComponent<GameDistributionHandler>();
#endif
#if CRAZYGAMES
            m_AdGetDetails = gameObject.AddComponent<CrazyAdsHandler>();
#endif
#if GAMEPIX
            m_AdGetDetails = gameObject.AddComponent<GamePixHandler>();
#endif
#if YANDEX
            m_AdGetDetails = gameObject.AddComponent<YandexHandler>();
#endif
#if GAMEMONETIZE
            m_AdGetDetails = gameObject.AddComponent<GameMonetizeHandler>();
#endif
#if WGPLAYGROUND
            m_AdGetDetails = gameObject.AddComponent<WGPlaygroundHandler>();
#endif
#if GAMEARTER
            m_AdGetDetails = gameObject.AddComponent<GameArterHandler>();
#endif
        }
        public bool IsRewardAdAvailable()
        {
            bool isAvailable = false;
            if (m_AdGetDetails != null)
            {
                isAvailable = m_AdGetDetails.IsAddAvailable(AdType.Reward);
            }
            return isAvailable;
        }
        public bool IsAdShowing()
        {
            bool isShoingAd = false;
            if (m_AdGetDetails != null)
            {
                isShoingAd = m_AdGetDetails.IsShowingAd();
            }
            return isShoingAd;
        }
        public void ShowInterstitial()
        {
            if (m_AdGetDetails != null)
            {
                m_AdGetDetails.ShowInstestitial("");
            }
        }
        public void ShowReward()
        {
            if (m_AdGetDetails != null)
            {
                m_AdGetDetails.ShowRewardAds("", () => { });
            }
        }
        public void ShowReward(Action onComplete)
        {
            if (m_AdGetDetails != null)
            {
                m_AdGetDetails.ShowRewardAds("", onComplete);
            }
        }
    }
}
