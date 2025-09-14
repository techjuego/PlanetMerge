using System;
using System.Collections;
using System.Collections.Generic;
using TechJuego.FruitSliceMerge.Monetization;
using UnityEngine;
namespace TechJuego.FruitSliceMerge
{
    public class GameArterHandler : MonoBehaviour, IAdGetDetail
    {
        public string GetAdId()
        {
            return string.Empty;
        }

        public void HideBanner()
        {
#if GAMEARTER
            Garter.I.RequestAd("banner1", Garter.BannerAction.Hide, Garter.BannerPosition.Bottom, Garter.BannerSize.Banner, (state) =>
            {
                StaticHelpersGarterSDK.SdkDebugger("Garter.I.RequestAd('banner1', Garter.BannerAction.Hide, Garter.BannerPosition.Bottom, Garter.BannerSize.Banner, callback)", "callback: " + state, "-");
            });
#endif
        }

        public bool IsAddAvailable(AdType adType)
        {
            bool isAdAvailable = false;
#if GAMEARTER
            isAdAvailable = Garter.I.RewardedAdAvailability();
#endif
            return isAdAvailable;
        }

        public bool IsShowingAd()
        {
            throw new NotImplementedException();
        }

        public void ShowBanner(string id)
        {
#if GAMEARTER
            Garter.I.RequestAd("banner1", Garter.BannerAction.Display, Garter.BannerPosition.Bottom, Garter.BannerSize.Banner, (state) =>
            {
                StaticHelpersGarterSDK.SdkDebugger("Garter.I.RequestAd('banner1', Garter.BannerAction.Display, Garter.BannerPosition.Top, Garter.BannerSize.Banner, callback)", "callback: " + state, "-");
            });
#endif
        }

        public void ShowInstestitial(string id)
        {
#if GAMEARTER
            Garter.I.RequestAd("fullscreen");
#endif
        }

        public void ShowRewardAds(string id, Action OnComplete)
        {
#if GAMEARTER
            Garter.I.RequestAd("rewarded", (state) =>
            {
                if (state == "completed")
                {
                    OnComplete?.Invoke();
                }
                StaticHelpersGarterSDK.SdkDebugger("Garter.I.RequestAd('rewarded', callback)", "callback: " + state, "-");
            });
#endif
        }
    }
}