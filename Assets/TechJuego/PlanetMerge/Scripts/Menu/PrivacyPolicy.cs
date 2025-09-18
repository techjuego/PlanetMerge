#if USE_ADMOB
using GoogleMobileAds.Ump.Api;
#endif
using TechJuego.PlanetMerge.Monetization;
using TechJuego.PlanetMerge.Sound;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

namespace TechJuego.PlanetMerge
{
    public class PrivacyPolicy : MonoBehaviour
    {
        [SerializeField] private Button m_PolicyButton;
        [SerializeField] private Button m_AcceptButton;
        public void OnEnable()
        {
            transform.SetAsLastSibling();

            m_PolicyButton.onClick.RemoveAllListeners();
            m_PolicyButton.onClick.AddListener(Button_Policy);

            m_AcceptButton.onClick.RemoveAllListeners();
            m_AcceptButton.onClick.AddListener(Button_Accept);
        }
        private void Button_Policy()
        {
            SoundEvents.OnPlaySingleShotSound?.Invoke("Click");
            Application.OpenURL("https://techjuego.com/privacypolicy/");
        }
        private void Button_Accept()
        {
            gameObject.SetActive(false);
            SoundEvents.OnPlaySingleShotSound?.Invoke("Click");
            MonetizationEvents.SetConcents(true);
#if UnityAds
            MetaData gdprMetaData = new MetaData("gdpr");
            gdprMetaData.Set("consent", "true");
            Advertisement.SetMetaData(gdprMetaData);

#endif

#if USE_ADMOB
            ConsentRequestParameters request = new ConsentRequestParameters
            {
                TagForUnderAgeOfConsent = false,
            };
            // Check the current consent information status.
            ConsentInformation.Update(request, OnConsentInfoUpdated);
#endif
            if (FindObjectOfType<MobileAdsHandler>() != null)
            {
                FindObjectOfType<MobileAdsHandler>().InitializeAds();
            }
        }
#if USE_ADMOB
        void OnConsentInfoUpdated(FormError consentError)
        {
            if (consentError != null)
            {
                // Handle the error.
                UnityEngine.Debug.LogError(consentError);
                return;
            }

            // If the error is null, the consent information state was updated.
            // You are now ready to check if a form is available.
            ConsentForm.Load((ConsentForm forms,FormError formError) =>
            {
                if (formError != null)
                {
                    // Consent gathering failed.
                   Debug.LogError(consentError);
                    return;
                }

                // Consent has been gathered.
            });
        }
#endif
    }
}