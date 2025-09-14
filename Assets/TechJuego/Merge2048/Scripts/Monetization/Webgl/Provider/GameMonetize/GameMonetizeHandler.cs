using System;
using System.Collections;
using System.Collections.Generic;
using TechJuego.FruitSliceMerge.Monetization;
using UnityEngine;

public class GameMonetizeHandler : MonoBehaviour, IAdGetDetail
{
    public string GetAdId()
    {
        return string.Empty;
    }

    public void HideBanner()
    {

    }

    public bool IsAddAvailable(AdType adType)
    {
        return true;
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
#if GAMEMONETIZE
        GameMonetize.Instance.ShowAd();
#endif
    }

    public void ShowRewardAds(string id, Action OnComplete)
    {

    }
}
