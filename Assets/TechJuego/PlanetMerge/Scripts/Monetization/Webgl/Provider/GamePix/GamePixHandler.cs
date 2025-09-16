using System;
using System.Collections;
using System.Collections.Generic;
#if GAMEPIX
using GamePix;
#endif
using UnityEngine;
namespace TechJuego.FruitSliceMerge.Monetization
{
    public class GamePixHandler : MonoBehaviour, IAdGetDetail
    {
        public string GetAdId()
        {
            return "";
        }

        public void HideBanner()
        {
        }

        public bool IsAddAvailable(AdType adType)
        {
            return true;
        }

        public void ShowBanner(string id)
        {

        }

        public void ShowInstestitial(string id)
        {
#if GAMEPIX
            Gpx.Ads.InterstitialAd(OnInterstitalAdSuccess);
#endif
        }
        private static Action onSuccess;
        public void ShowRewardAds(string id, Action OnComplete)
        {
            onSuccess = OnComplete;
#if GAMEPIX
            Gpx.Ads.RewardAd(OnRewardAdSuccess, OnRewardAdFail);
#endif
        }
#if GAMEPIX
        [AOT.MonoPInvokeCallback(typeof(Gpx.gpxCallback))]
        public static void OnInterstitalAdSuccess()
        {
            Gpx.Log("SUCCESS");
        }

        [AOT.MonoPInvokeCallback(typeof(Gpx.gpxCallback))]
        public static void OnRewardAdSuccess()
        {
            onSuccess?.Invoke();
            onSuccess = null;
        }

        [AOT.MonoPInvokeCallback(typeof(Gpx.gpxCallback))]
        public static void OnRewardAdFail()
        {
            onSuccess = null;
        }
#endif
        public bool IsShowingAd()
        {
            bool isShowingAds = false;
#if GAMEPIX
            isShowingAds = Gpx.IsGamePaused;
#endif
            return isShowingAds;
        }

        public void LoadBanner()
        {
           
        }
    }

}