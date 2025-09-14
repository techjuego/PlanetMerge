
using System.Collections.Generic;
using System;
using UnityEngine;

namespace TechJuego.FruitSliceMerge.Monetization
{
    public class MobileAdsHandler : MonoBehaviour
    {
        public static MobileAdsHandler Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MobileAdsHandler();
                }
                return _instance;
            }
        }
        private static MobileAdsHandler _instance;
        public MobileAdsHandler()
        {
            _instance = this;
        }
        public bool testMode;
        public MobileAdsData m_MobileAdsData;
        public List<IAdGetDetail> adGetDetails = new List<IAdGetDetail>();
        private void Awake()
        {
            if (m_MobileAdsData == null)
            {
                m_MobileAdsData = Resources.Load("Monetization/MobileAdsData") as MobileAdsData;
                if (m_MobileAdsData != null)
                {
                    if (m_MobileAdsData.adsEvents.Count > 0)
                    {
                        foreach (var item in m_MobileAdsData.adsEvents)
                        {
                            item.calls = 0;
                        }
                    }
                }
            }
        }
        public void Load() { }
        private void OnEnable()
        {
            if (MonetizationEvents.HasConcentSet())
            {
                InitializeAds();
            }
        }
        public void InitializeAds()
        {
#if UnityAds
            gameObject.AddComponent<UnityAdsInitializer>().Initialize();
#endif
            gameObject.AddComponent<AdmobAdsInitializer>().Initialize();
        }
        public int InterstitialCount = 0;
        public bool IsInterstitialAdAvailable()
        {
            List<IAdGetDetail> tempList = new List<IAdGetDetail>();
            foreach (var item in adGetDetails)
            {
                if (item.IsAddAvailable(AdType.Interstitial))
                {
                    tempList.Add(item);
                }
            }
            return tempList.Count > 0;
        }
        public void ShowInterstitial()
        {
#if UNITY_WEBGL

#else
            List<IAdGetDetail> tempList = new List<IAdGetDetail>();
            foreach (var item in adGetDetails)
            {
                if (item.IsAddAvailable(AdType.Interstitial))
                {
                    tempList.Add(item);
                }
            }
            if (tempList.Count > 0)
            {
                InterstitialCount++;
                if (InterstitialCount >= tempList.Count)
                {
                    InterstitialCount = 0;
                }
                tempList[InterstitialCount].ShowInstestitial(tempList[InterstitialCount].GetAdId());
            }
#endif
        }
        public int BannerCount = 0;
        public void ShowBanner()
        {
            foreach (var item in adGetDetails)
            {
                if (item.IsAddAvailable(AdType.Reward))
                {
                    item.ShowBanner(item.GetAdId());
                    break;
                }
            }
        }
        public int RewardCount = 0;
        public bool IsWebGlRewardAdAvailable()
        {
            return true;
        }

        public bool IsRewardAdAvailable()
        {
            bool isPresent = true;
            List<IAdGetDetail> tempList = new List<IAdGetDetail>();
            foreach (var item in adGetDetails)
            {
                if (item.IsAddAvailable(AdType.Reward))
                {
                    tempList.Add(item);
                }
            }
            isPresent = tempList.Count > 0;
            return isPresent;
        }
        public void ShowReward()
        {

            List<IAdGetDetail> tempList = new List<IAdGetDetail>();
            foreach (var item in adGetDetails)
            {
                if (item.IsAddAvailable(AdType.Reward))
                {
                    tempList.Add(item);
                }
            }
            if (tempList.Count > 0)
            {
                RewardCount++;
                if (RewardCount >= tempList.Count)
                {
                    RewardCount = 0;
                }
                tempList[RewardCount].ShowRewardAds(tempList[RewardCount].GetAdId(), () => { });
            }
        }
        public void ShowReward(Action onComplete)
        {
            List<IAdGetDetail> tempList = new List<IAdGetDetail>();
            foreach (var item in adGetDetails)
            {
                if (item.IsAddAvailable(AdType.Reward))
                {
                    tempList.Add(item);
                }
            }
            if (tempList.Count > 0)
            {
                RewardCount++;
                if (RewardCount >= tempList.Count)
                {
                    RewardCount = 0;
                }
                tempList[RewardCount].ShowRewardAds(tempList[RewardCount].GetAdId(), onComplete);
            }
        }
    }
}