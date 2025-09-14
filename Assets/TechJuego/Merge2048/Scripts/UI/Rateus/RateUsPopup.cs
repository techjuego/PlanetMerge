using UnityEngine;
using UnityEngine.UI;
using TechJuego.FruitSliceMerge.Utils;
using TechJuego.FruitSliceMerge.Rateus;
using TechJuego.FruitSliceMerge.Sound;

namespace TechJuego.FruitSliceMerge.Rateus
{
    // Class responsible for handling the "Rate Us" popup
    public class RateUsPopup : MonoBehaviour
    {
        // Reference to the RateUsData settings
        private RateUsData m_RateUsData;

        // Button to delay the rating prompt
        [SerializeField] private Button m_LaterButton;

        // Button to open the app store for rating
        [SerializeField] private Button m_RateUsButton;

        // Called when the script is enabled
        private void OnEnable()
        {
            // Ensure the popup is displayed on top of other UI elements
            transform.SetAsLastSibling();

            // Load the RateUsData settings if not already loaded
            if (m_RateUsData == null)
            {
                m_RateUsData = Resources.Load<RateUsData>("Rateus/RateUsData");
            }

            // Set up button click handlers
            UiUtility.SetButton(m_LaterButton, OnClickLaterButton);
            UiUtility.SetButton(m_RateUsButton, OnClickRateUsButton);
        }

        // Handler for the "Later" button click
        private void OnClickLaterButton()
        {
            // Hide the popup
            gameObject.SetActive(false);

            // Play a sound effect for the button click
            SoundEvents.OnPlaySingleShotSound?.Invoke("Click");
        }

        // Handler for the "Rate Us" button click
        private void OnClickRateUsButton()
        {
            // Play a sound effect for the button click
            SoundEvents.OnPlaySingleShotSound?.Invoke("Click");

            // Open the app store page for rating
            Application.OpenURL(m_RateUsData.googlePlayBundleID);

#if UNITY_ANDROID
            // For Android, open the Google Play Store page
            Application.OpenURL("https://play.google.com/store/apps/details?id=" + m_RateUsData.googlePlayBundleID);
#endif

#if UNITY_IOS
            // For iOS, open the App Store page
            Application.OpenURL("https://itunes.apple.com/app/id" + m_RateUsData.iosAppID);
#endif

            // Mark that the user has been prompted to rate
            RateUsSetting.SetRateDone();
        }
    }
}
