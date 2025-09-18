using UnityEngine;
using System;

namespace TechJuego.PlanetMerge
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
        public int GetHighScore()
        {
          return PlayerPrefs.GetInt("HighScore");
        }
        public  void SetHighScore(int score)
        {
                    PlayerPrefs.SetInt("HighScore", score);  // Save the new high score
        }
    }
}
