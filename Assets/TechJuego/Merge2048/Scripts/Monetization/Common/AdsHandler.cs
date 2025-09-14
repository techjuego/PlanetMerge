using System;
using TechJuego.FruitSliceMerge.Monetization;
namespace TechJuego.FruitSliceMerge
{
    public class AdsHandler : Singleton<AdsHandler>
    {
        protected AdsHandler() { }
        private WebglAdsHandler m_WebglAdsHandler;
        private MobileAdsHandler m_MobileAdsHandler;
        private MobileAdsData m_MobileAdsData;
        private WebglAdsData m_WebglAdsData;
        public void Load()
        {
            m_MobileAdsData = ResourcesRef.GetMobileAdsData();
            m_WebglAdsData = ResourcesRef.GetWebglAdsData();
#if UNITY_ANDROID || UNITY_IPHONE
            m_MobileAdsHandler = gameObject.AddComponent<MobileAdsHandler>();
#endif
#if UNITY_WEBGL
            m_WebglAdsHandler = gameObject.AddComponent<WebglAdsHandler>();
#endif
        }
        public void ShowInterstitial()
        {
#if UNITY_ANDROID || UNITY_IPHONE
#endif
#if UNITY_WEBGL
            m_WebglAdsHandler.ShowInterstitial();
#endif
        }
        public void ShowReward()
        {
#if UNITY_ANDROID || UNITY_IPHONE
       
#endif
#if UNITY_WEBGL
            m_WebglAdsHandler.ShowReward();
#endif
        }
        public void ShowReward(Action onComplete)
        {
#if UNITY_ANDROID || UNITY_IPHONE
       
#endif
#if UNITY_WEBGL
            m_WebglAdsHandler.ShowReward(onComplete);
#endif
        }

        public bool IsAdShowing()
        {
            bool isShoingAd = false;
#if UNITY_WEBGL
            isShoingAd = m_WebglAdsHandler.IsAdShowing();
#endif
            return isShoingAd;
        }

        public void ShowAds(GameState gameState)
        {
#if UNITY_ANDROID || UNITY_IPHONE
            // Check for ad events based on the current game state
            foreach (var item in m_MobileAdsData.adsEvents)
            {
                if (item.gameEvent == gameState)
                {
                    // Increment ad call count
                    item.calls++;

                    // Show ads if the call count matches the specified interval
                    if (item.calls % item.everyLevel == 0)
                    {
                        ShwoMobileAds(item.AddToCall);
                    }
                }
            }
#endif
#if UNITY_WEBGL
            foreach (var item in m_WebglAdsData.webglAdEvents)
            {
                if (item.gameEvent == gameState)
                {
                    // Increment ad call count
                    item.calls++;

                    // Show ads if the call count matches the specified interval
                    if (item.calls % item.everyLevel == 0)
                    {
                        ShwoMobileAds(item.AddToCall);
                    }
                }
            }
#endif
        }
        void ShwoMobileAds(AdType adType)
        {
            // Display the ad based on its type
            switch (adType)
            {
                case AdType.Interstitial:
                    // Show an interstitial ad
                    ShowInterstitial();
                    break;
                case AdType.Reward:
                    // Show a reward ad
                    ShowReward();
                    break;
            }
        }
    }
}