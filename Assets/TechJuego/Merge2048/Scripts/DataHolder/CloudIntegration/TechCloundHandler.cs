using System;
using System.Collections;
using System.Collections.Generic;

namespace TechJuego.FruitSliceMerge.Integration
{

    public class TechCloundHandler : Singleton<TechCloundHandler>
    {
        protected TechCloundHandler()
        {

        }
        public UserDetail m_UserDetail = new UserDetail();
        public int HighScore;
        private PlayfabHandler playfabHandler;
        public void Load()
        {
            playfabHandler = gameObject.AddComponent<PlayfabHandler>();
            Login();
        }
        public void Login()
        {
            playfabHandler.Login(()=> { });
        }

        public void SaveHighscore(int highScore)
        {
            playfabHandler.SetHighScore(highScore, () => { });
        }
        public void GetHighScore(Action onComplete)
        {
            playfabHandler.GetHighScore((value) => 
            {
                highScoreList = value;
                onComplete?.Invoke();
            });
        }
        public void GetOurScore(Action<HighScoreData> result)
        {
            HighScoreData highScoreData = new HighScoreData();
            for (int i = 0; i < highScoreList.Count; i++)
            {
                if (highScoreList[i].UserId == m_UserDetail.UserId)
                {
                    highScoreData = highScoreList[i];
                    break;
                }
            }
            result?.Invoke(highScoreData);
        }
        public List<HighScoreData> highScoreList = new List<HighScoreData>();
    }
}