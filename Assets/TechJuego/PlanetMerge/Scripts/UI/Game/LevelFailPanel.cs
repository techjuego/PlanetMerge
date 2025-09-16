using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TechJuego.FruitSliceMerge.Utils;
using TechJuego.FruitSliceMerge.Sound;
using TechJuego.FruitSliceMerge.Monetization;

namespace TechJuego.FruitSliceMerge
{
    public class LevelFailPanel : MonoBehaviour
    {
        [SerializeField] private Button m_ReplayButton;
        [SerializeField] private Button m_HomeButton;
        [SerializeField] private TextMeshProUGUI m_Score;
        [SerializeField] private TextMeshProUGUI m_BestScore;
        private void OnEnable()
        {
            UiUtility.SetButton(m_ReplayButton, OnClickRelayButton);
            UiUtility.SetButton(m_HomeButton, OnClickHomeButton);
            DataHandler.Instance.SetHighScore(GameManager.Instance.Score);
            m_Score.text = GameManager.Instance.Score.ToString();
            m_BestScore.text = "Best:-" + DataHandler.Instance.GetHighScore().ToString();
            AdsHandler.Instance.ShowBanner();
        }
        void OnClickRelayButton()
        {
            SoundEvents.OnPlaySingleShotSound?.Invoke("Click");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        void OnClickHomeButton()
        {
            SoundEvents.OnPlaySingleShotSound?.Invoke("Click");
            SceneLoader.LoadScene("Menu", GameColors.Color1(), 1.5f, ChangeEffect.BottomFill);
        }
    }
}