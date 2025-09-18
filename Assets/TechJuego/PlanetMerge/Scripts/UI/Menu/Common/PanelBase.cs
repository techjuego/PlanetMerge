using UnityEngine;
using UnityEngine.UI;
using TechJuego.PlanetMerge.Utils;
using TechJuego.PlanetMerge.Sound;

namespace TechJuego.PlanetMerge
{
    public class PanelBase : MonoBehaviour
    {
        public Button m_SettingButton;
        public GameObject m_Title;
        private void OnEnable()
        {
            transform.SetAsLastSibling();
            UiUtility.SetButton(m_SettingButton, OnClickSettingButton);
            TechTween.TrignometricRotate(m_Title, new Vector3(0, 0, 5), new Vector3(0, 0, 1.5f));
        }
        private void OnClickSettingButton()
        {
            SoundEvents.OnPlaySingleShotSound?.Invoke("Click");
            MainMenuPanel.Instance.m_SettingPanel.gameObject.SetActive(true);
        }
    }
}