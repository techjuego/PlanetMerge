#if CRAZYGAMES
using CrazyGames;
#endif
using UnityEngine;
namespace TechJuego.FruitSliceMerge
{
    public class CrazyGamesIntegration : Singleton<DataHandler>
    {
        private void OnEnable()
        {
#if CRAZYGAMES
            CrazySDK.Init(() => 
            {
                CrazySDK.User.GetUser(user =>
                {
                    if (user != null)
                    {
                        Debug.Log("Get user result: " + user);
                    }
                    else
                    {
                        Debug.Log("User is not logged in");
                    }
                });
            });
#endif
        }
        public void SetHighScore(int Score)
        {
#if CRAZYGAMES
            CrazySDK.Data.SetInt("HighScore", Score);
#endif
        }
        public int GetHighScore()
        {
            int score = 0;
#if CRAZYGAMES
           score =  CrazySDK.Data.GetInt("HighScore");
#endif
            return score;
        }
    }
}