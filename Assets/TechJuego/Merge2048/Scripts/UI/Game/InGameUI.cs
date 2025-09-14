using UnityEngine;
namespace TechJuego.FruitSliceMerge
{
    public class InGameUI : MonoBehaviour
    {
        public static InGameUI Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new InGameUI();
                }
                return _instance;
            }
        }
        private static InGameUI _instance;
        public InGameUI()
        {
            _instance = this;
        }
        [SerializeField] private HUDPanel m_HUDUI;
        [SerializeField] private RewardVideo m_RewardVideo;
        [SerializeField] private LevelFailPanel m_LevelFailUI;
        [SerializeField] private InGameSettingsPanel m_InGameSettingsPanel;
        private void OnEnable()
        {
            m_HUDUI.gameObject.SetActive(true);
            m_LevelFailUI.gameObject.SetActive(false);
            m_RewardVideo.gameObject.SetActive(false);
            m_InGameSettingsPanel.gameObject.SetActive(false);
            GameEvents.OnGameEnd += GameEvents_OnGameEnd;
            AudioListener.volume = GameSetting.GetSound() ? 1 : 0;
            GameEvents.OnGetBooster += GameEvents_OnGetBooster;
        }
        private void OnDisable()
        {
            GameEvents.OnGameEnd -= GameEvents_OnGameEnd;
            GameEvents.OnGetBooster -= GameEvents_OnGetBooster;
        }
        private void GameEvents_OnGetBooster(Booster booster)
        {
            m_RewardVideo.ShowReward(booster);
        }
        private void GameEvents_OnGameEnd()
        {
            m_LevelFailUI.gameObject.SetActive(true);
        }
        public void ShowSettingPanel()
        {
            m_InGameSettingsPanel.gameObject.SetActive(true);
        }
    }
}