using TechJuego.FruitSliceMerge.Sound;
using TechJuego.FruitSliceMerge.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace TechJuego.FruitSliceMerge
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
            SceneLoader.LoadScene("Game", GameColors.Color1(), 2, ChangeEffect.BottomFill);
        }
    }
}