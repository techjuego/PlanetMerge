using System;
#if PLAYFAB
using PlayFab;
using PlayFab.ClientModels;
#endif
using UnityEngine;

using System.Collections.Generic;

namespace TechJuego.FruitSliceMerge.Integration
{
    public class PlayfabHandler : MonoBehaviour, ICloudIntegration
    {
        public void Login(Action OnComplete)
        {
            #if PLAYFAB
            PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
            {
                CustomId = SystemInfo.deviceUniqueIdentifier
            },
            (result) =>
            {
                TechCloundHandler.Instance.m_UserDetail.UserId = result.PlayFabId;
            },
            (error) =>
            {

            });
#endif
        }
        public void SetHighScore(int score, Action OnComplete)
        {
            #if PLAYFAB
            PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest()
            {
                Statistics = new System.Collections.Generic.List<StatisticUpdate>() 
                { 
                    new StatisticUpdate(){ StatisticName = "FruitMerge" , Value = score}
                }
            },
            (result) =>
            {
                Debug.Log(result.ToJson());
            },
            (error) =>
            {

            });
#endif
        }
        public void GetHighScore(Action<List<HighScoreData>> OnComplete)
        {
#if PLAYFAB
            PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
            {
                StatisticName = "FruitMerge",
                StartPosition = 0,
                MaxResultsCount = 10
            },
            (result) =>
            {
                List<HighScoreData> highScores = new List<HighScoreData>();
                Debug.Log("Leaderboard retrieved!");
                foreach (var entry in result.Leaderboard)
                {
                    Debug.Log(entry.ToJson());
                    highScores.Add(new HighScoreData() {  Index= entry.Position+1, Score = entry.StatValue, UserId = entry.PlayFabId });
                }
                OnComplete?.Invoke(highScores);
            },
            (error) =>
            {

            });
#endif
        }
    }
}