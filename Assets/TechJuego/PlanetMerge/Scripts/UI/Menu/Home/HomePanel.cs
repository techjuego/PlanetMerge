using TechJuego.PlanetMerge.Monetization;
using TechJuego.PlanetMerge.Sound;
using TechJuego.PlanetMerge.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace TechJuego.PlanetMerge
{
    public class HomePanel : MonoBehaviour
    {
        [SerializeField] private Button m_PlayButton;
        private void OnEnable()
        {
            UiUtility.SetButton(m_PlayButton, OnClickPlayButton);
        }
        private void OnClickPlayButton()
        {
            SoundEvents.OnPlaySingleShotSound?.Invoke("Click");
            AdsHandler.Instance.HideBanner();
            SceneLoader.LoadScene("Game", GameColors.Color1(), 2, ChangeEffect.BottomFill);
        }
    }
}