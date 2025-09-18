using UnityEngine;
#if UnityAds
using UnityEngine.Advertisements;

namespace TechJuego.PlanetMerge.Monetization
{
    public class UnityAdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
    {

        string gameid;
        public void Initialize()
        {
            if (!string.IsNullOrEmpty(MobileAdsHandler.Instance.m_MobileAdsData.UnityAppID_Android))
            {
#if UNITY_ANDROID
                gameid = MobileAdsHandler.Instance.m_MobileAdsData.UnityAppID_Android;
#endif
                Advertisement.Initialize(gameid, MobileAdsHandler.Instance.testMode, this);
            }
            if (!string.IsNullOrEmpty(MobileAdsHandler.Instance.m_MobileAdsData.AdmobAppID_IOS))
            {
#if UNITY_IPHONE
                gameid = MobileAdsHandler.Instance.m_MobileAdsData.AdmobAppID_IOS;
#endif
                Advertisement.Initialize(gameid, MobileAdsHandler.Instance.testMode, this);
            }
        }

        public void OnInitializationComplete()
        {
            foreach (var item in MobileAdsHandler.Instance.m_MobileAdsData.monitizationAds)
            {
                Debug.Log(item);
                if (item.providers == "Unity")
                {
                    UnityAdHandler unityAdHandler = gameObject.AddComponent<UnityAdHandler>();
                    unityAdHandler.adType = item.AdType;
#if UNITY_ANDROID
                    unityAdHandler._adUnitId = item.Android_ID;
#elif UNITY_IPHONE
                    unityAdHandler._adUnitId = item.IOS_ID;
#endif
                    unityAdHandler.Initialize();
                    MobileAdsHandler.Instance.adGetDetails.Add(unityAdHandler);
                }
            }
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
        }
    }
}
#endif