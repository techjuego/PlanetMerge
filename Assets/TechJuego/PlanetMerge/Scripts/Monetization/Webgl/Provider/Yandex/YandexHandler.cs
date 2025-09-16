using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if YANDEX
using YG;
#endif

namespace TechJuego.FruitSliceMerge.Monetization
{
    public class YandexHandler : MonoBehaviour, IAdGetDetail
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

        public void LoadBanner()
        {
           
        }

        public void ShowBanner(string id)
        {

        }

        public void ShowInstestitial(string id)
        {
#if YANDEX
            YG2.InterstitialAdvShow();
#endif
        }

        public void ShowRewardAds(string id, Action OnComplete)
        {
#if YANDEX
            YG2.RewardedAdvShow("Ads", OnComplete);
#endif
        }
    }
}