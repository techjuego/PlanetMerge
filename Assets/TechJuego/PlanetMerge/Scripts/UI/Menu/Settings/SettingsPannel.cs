using TechJuego.PlanetMerge;
//using TechJuego.PlanetMerge.HapticFeedback;
using TechJuego.PlanetMerge.Sound;
using TechJuego.PlanetMerge.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace TechJuego.PlanetMerge
{
    // Class responsible for managing the settings panel
    public class SettingsPannel : MonoBehaviour
    {
        // Button to return to the previous screen
        [SerializeField] private Button m_BackButton;

        // Button to toggle sound settings
        [SerializeField] private SwitchButton m_MusicButton;

        [SerializeField] private SwitchButton m_SfxButton;

        [SerializeField] private SwitchButton m_Vibration;

        // Button to open the privacy policy
        [SerializeField] private Button m_PrivacyPolicyButton;

        [SerializeField] private RectTransform m_Panel;
        // Called when the script is enabled
        private void OnEnable()
        {
            transform.SetAsLastSibling();
            m_PrivacyPolicyButton.gameObject.SetActive(false);
            // Set up button click handlers
            UiUtility.SetButton(m_BackButton, CloseSettingPannel);
            UiUtility.SetButton(m_PrivacyPolicyButton, OpenPolicy);

            // Initialize the sound toggle button with the sound setting key
            m_MusicButton.Initialize(SoundSetting.MusicVariable);

            // Clear any existing listeners and add a new one for the sound button
            m_MusicButton.OnClicEvent.RemoveAllListeners();
            m_MusicButton.OnClicEvent.AddListener(Button_Music);

            m_SfxButton.Initialize(SoundSetting.SfxVariable);

            m_SfxButton.OnClicEvent.RemoveAllListeners();
            m_SfxButton.OnClicEvent.AddListener(Button_SFX);

            GameDistribution.Instance.SendEvent("Settings");

           // m_Vibration.Initialize(HapticSetting.HapticViration);

            // Ensure the settings panel is displayed on top of other UI elements
            transform.SetAsLastSibling();

            OpenEffect();
#if UNITY_ANDROID || UNITY_IPHONE
 m_PrivacyPolicyButton.gameObject.SetActive(true);
#endif
        }

        // Handler for the sound toggle button click
        void Button_Music(bool value)
        {
            SoundEvents.OnMusicSettingUpdate?.Invoke();
            // Adjust the AudioListener volume based on the sound setting
        }
        void Button_SFX(bool value)
        {
            // Adjust the AudioListener volume based on the sound setting
        }

        // Method to close the settings panel
        public void CloseSettingPannel()
        {
            SoundEvents.OnPlaySingleShotSound?.Invoke("Click");
            TechTween.MoveTo(m_Panel, new Vector3(Screen.width * 3, 0, 0), 0.5f, true).GetCompleteCallback(() =>
            {
                gameObject.SetActive(false);
            });

        }

        // Method to open the privacy policy
        void OpenPolicy()
        {
            // Play a sound effect for the button click
            SoundEvents.OnPlaySingleShotSound?.Invoke("Click");
            Application.OpenURL("https://techjuego.com/privacypolicy/");
        }
        void OpenEffect()
        {
            m_Panel.anchoredPosition = new Vector2(Screen.width*3, 0);
            TechTween.MoveTo(m_Panel, Vector3.zero, 0.5f, true);
        }
    }
}
