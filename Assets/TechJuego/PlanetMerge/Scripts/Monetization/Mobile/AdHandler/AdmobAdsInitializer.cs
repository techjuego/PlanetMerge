#if ADMOB
using GoogleMobileAds.Api;
#endif
using UnityEngine;
namespace TechJuego.PlanetMerge.Monetization
{
    public class AdmobAdsInitializer : MonoBehaviour
    {
        public void Initialize()
        {
#if ADMOB
            MobileAds.Initialize((InitializationStatus initStatus) =>
            {
                foreach (var item in MobileAdsHandler.Instance.m_MobileAdsData.monitizationAds)
                {
                    if (item.providers == "Admob")
                    {
                        AdmobHandler admobHandler = gameObject.AddComponent<AdmobHandler>();
#if UNITY_ANDROID
                        admobHandler._adUnitId = item.Android_ID;
#elif UNITY_IPHONE
                        admobHandler._adUnitId = item.IOS_ID;
#endif
                        admobHandler.adType = item.AdType;
                        admobHandler.Initialize();
                        MobileAdsHandler.Instance.adGetDetails.Add(admobHandler);
                    }
                }
            });
#endif
        }
    }
}