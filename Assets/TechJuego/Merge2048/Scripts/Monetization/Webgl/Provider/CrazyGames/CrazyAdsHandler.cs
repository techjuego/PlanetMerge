using System;
using UnityEngine;
#if CRAZYGAMES
using CrazyGames;
#endif
namespace TechJuego.FruitSliceMerge.Monetization
{
    public class CrazyAdsHandler : MonoBehaviour, IAdGetDetail
    {
        public string GetAdId()
        {
            return "";
        }

        public void HideBanner()
        {

        }
        private void Awake()
        {
#if CRAZYGAMES
            CrazySDK.Init(() => { });
#endif
        }

        public bool IsAddAvailable(AdType adType)
        {
            bool isPresent = false;
            switch (adType)
            {
                case AdType.Banner:
                    break;
                case AdType.Interstitial:
                    break;
                case AdType.Reward:
                    break;
            }
            return isPresent;
        }

        public bool IsShowingAd()
        {
            return false;
        }

        public void ShowBanner(string id)
        {

        }

        public void ShowInstestitial(string id)
        {
#if CRAZYGAMES
            CrazySDK.Ad.RequestAd(
                  CrazyAdType.Midgame,
                  () =>
                  {
                      Debug.Log("Rewarded ad started");
                  },
                  (error) =>
                  {
                      Debug.Log("Rewarded ad error: " + error);
                  },
                  () =>
                  {
                      Debug.Log("Rewarded ad finished, reward the player here");
                  }
              );
#endif
        }

        public void ShowRewardAds(string id, Action OnComplete)
        {
#if CRAZYGAMES

            CrazySDK.Ad.RequestAd(
                CrazyAdType.Rewarded,
                () =>
                {
                    Debug.Log("Rewarded ad started");
                },
                (error) =>
                {
                    Debug.Log("Rewarded ad error: " + error);
                },
                () =>
                {
                    Debug.Log("Rewarded ad finished, reward the player here");
                    OnComplete?.Invoke();
                }
            );
#endif
        }
    }
}