using TechJuego.FruitSliceMerge.Monetization;
using TechJuego.FruitSliceMerge.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TechJuego.FruitSliceMerge
{
    public class MainMenuPanel : MonoBehaviour
    {
        public static MainMenuPanel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MainMenuPanel();
                }
                return _instance;
            }
        }
        private static MainMenuPanel _instance;
        public MainMenuPanel()
        {
            _instance = this;
        }
        public PanelBase m_MenuPanel;
        public SettingsPannel m_SettingPanel;
        public PrivacyPolicy m_privacyPolicy;
        private void OnEnable()
        {
            m_MenuPanel.gameObject.SetActive(true);
            m_SettingPanel.gameObject.SetActive(false);
#if UNITY_ANDROID || UNITY_IPHONE
            if (MonetizationEvents.HasConcentSet())
            {
                m_privacyPolicy.gameObject.SetActive(false);
            }
            else
            {
                m_privacyPolicy.gameObject.SetActive(true);
            }
#endif
            SoundEvents.OnPlayLoopSound?.Invoke("BGMUSIC");
        }
    }
}