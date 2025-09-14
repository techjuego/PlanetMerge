using UnityEngine;
using System;

namespace TechJuego.FruitSliceMerge
{
    // Singleton class for handling game data
    public class DataHandler : Singleton<DataHandler>
    {
        // Protected constructor to prevent instantiation from outside
        protected DataHandler()
        {
        }
        public int BombCount
        {
            get
            {
                return PlayerPrefs.GetInt("BOMB", 3);
            }
            set
            {
                PlayerPrefs.SetInt("BOMB", value);
                GameEvents.OnUpdateBombCount?.Invoke();
            }
        }
        // Method to get the current high score from PlayerPrefs
        public void GetHighScore(Action<int> score)
        {
            CrazyGamesIntegration.Instance.GetHighScore(score);
        }
        // Method to set a new high score in PlayerPrefs if the given score is higher
        public  void SetHighScore(int score)
        {
            // If the new score is higher than the current high score, update it in PlayerPrefs
            GetHighScore((val)=> 
            {
                if(score > val)
                {
                    CrazyGamesIntegration.Instance.SetHighScore(score);
                    PlayerPrefs.SetInt("HighScore", score);  // Save the new high score
                }
            });
        }
    }
}
