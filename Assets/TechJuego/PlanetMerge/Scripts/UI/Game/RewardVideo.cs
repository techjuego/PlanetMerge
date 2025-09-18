using UnityEngine;
using UnityEngine.UI;
using TechJuego.PlanetMerge.Utils;
using TechJuego.PlanetMerge.Monetization;

namespace TechJuego.PlanetMerge
{
    public class RewardVideo : MonoBehaviour
    {
        [SerializeField] private Button m_RewardVideo;
        private Booster m_Booster;
        private void OnEnable()
        {
            transform.SetAsLastSibling();
            UiUtility.SetButton(m_RewardVideo, OnClickRewardVideo);
        }
        private void OnClickRewardVideo()
        {
            AdsHandler.Instance.ShowReward(()=> 
            {
                switch (m_Booster)
                {
                    case Booster.Bomb:
                        DataHandler.Instance.BombCount++;
                        break;
                }
                gameObject.SetActive(false);
            });
        }
        public void ShowReward(Booster booster)
        {
            m_Booster = booster;
            gameObject.SetActive(true);
        }
    }
}