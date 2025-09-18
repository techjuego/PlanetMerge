using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TechJuego.PlanetMerge.Monetization
{
    public class GameDistributionHandler : MonoBehaviour, IAdGetDetail
    {
        protected GameDistributionHandler() { }

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
            return true;
        }

        public void ShowBanner(string id)
        {
        }

        public void ShowInstestitial(string id)
        {
#if GAMEDISTRIBUTION
            GameDistribution.Instance.ShowAd();
#endif
        }
        private Action OnCompleteReward;
        public void ShowRewardAds(string id, Action OnComplete)
        {
            OnCompleteReward = OnComplete;
#if GAMEDISTRIBUTION
            GameDistribution.Instance.ShowRewardedAd();
#endif
        }
        private void OnEnable()
        {
            #if GAMEDISTRIBUTION
            GameDistribution.OnResumeGame += OnResumeGame;
            GameDistribution.OnPauseGame += OnPauseGame;
            GameDistribution.OnPreloadRewardedVideo += OnPreloadRewardedVideo;
            GameDistribution.OnRewardedVideoSuccess += OnRewardedVideoSuccess;
            GameDistribution.OnRewardedVideoFailure += OnRewardedVideoFailure;
            GameDistribution.OnRewardGame += OnRewardGame;
            GameDistribution.OnEvent += OnEvent;
#endif
        }
        private void OnDisable()
        {
            #if GAMEDISTRIBUTION
            GameDistribution.OnResumeGame += OnResumeGame;
            GameDistribution.OnPauseGame += OnPauseGame;
            GameDistribution.OnPreloadRewardedVideo += OnPreloadRewardedVideo;
            GameDistribution.OnRewardedVideoSuccess += OnRewardedVideoSuccess;
            GameDistribution.OnRewardedVideoFailure += OnRewardedVideoFailure;
            GameDistribution.OnRewardGame += OnRewardGame;
            GameDistribution.OnEvent += OnEvent;
#endif
        }
        private void OnEvent(string obj)
        {
            
        }

        private void OnRewardGame()
        {
            
        }

        private void OnRewardedVideoFailure()
        {
         
        }

        private void OnRewardedVideoSuccess()
        {
            OnCompleteReward?.Invoke();
        }

        private void OnPreloadRewardedVideo(int obj)
        {
            
        }

        private void OnPauseGame()
        {
           
        }

        private void OnResumeGame()
        {
            
        }

        public void LoadBanner()
        {
           
        }
    }

}