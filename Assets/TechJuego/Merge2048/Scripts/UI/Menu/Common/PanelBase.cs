using UnityEngine;
using UnityEngine.UI;
using TechJuego.FruitSliceMerge.Utils;
using TechJuego.FruitSliceMerge.Sound;

namespace TechJuego.FruitSliceMerge
{
    public class PanelBase : MonoBehaviour
    {
        public Button m_SettingButton;
        public Button m_HighScoreButton;
        public GameObject m_Title;
        private void OnEnable()
        {
            transform.SetAsLastSibling();
            UiUtility.SetButton(m_SettingButton, OnClickSettingButton);
            UiUtility.SetButton(m_HighScoreButton, OnClickHighScoreButton);
            TechTween.TrignometricRotate(m_Title, new Vector3(0, 0, 5), new Vector3(0, 0, 1.5f));
        }
        private void OnClickSettingButton()
        {
            SoundEvents.OnPlaySingleShotSound?.Invoke("Click");
            MainMenuPanel.Instance.m_SettingPanel.gameObject.SetActive(true);
        }
        private void OnClickHighScoreButton()
        {
            SoundEvents.OnPlaySingleShotSound?.Invoke("Click");
            MainMenuPanel.Instance.ShowHighScorePanel();
        }
    }
}