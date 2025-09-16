using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TechJuego.FruitSliceMerge.Utils;
using TechJuego.FruitSliceMerge.Sound;

namespace TechJuego.FruitSliceMerge
{
    public class HUDPanel : MonoBehaviour
    {
        [SerializeField] private Button m_BombButton;
        [SerializeField] private Button m_SettingButton;
        [SerializeField] private TextMeshProUGUI m_BombCount;
        [SerializeField] private TextMeshProUGUI m_Score;
        private void OnEnable()
        {
            m_BombButton.gameObject.SetActive(false);
            UiUtility.SetButton(m_BombButton, OnClickBombButton);
            UiUtility.SetButton(m_SettingButton, OnClickSettingButton);
            GameEvents.OnUpdateBombCount += GameEvents_OnUpdateBombCount;
            GameEvents.OnUpdateScore += GameEvents_OnUpdateScore;
            GameEvents_OnUpdateBombCount();
        }
        private void OnDisable()
        {
            GameEvents.OnUpdateBombCount -= GameEvents_OnUpdateBombCount;
            GameEvents.OnUpdateScore -= GameEvents_OnUpdateScore;
        }
        private void Start()
        {
            m_Score.text = GameManager.Instance.Score.ToString();
        }
        private void GameEvents_OnUpdateScore()
        {
            var lastValue = float.Parse(m_Score.text);
            TechTween.ScaleTo(m_Score.gameObject, Vector3.one, 0.3f).SetEaseType(EaseTween.EaseInOutBounce);
            StartCoroutine(UpdateScore(lastValue, GameManager.Instance.Score, 1, (val) =>
            {
                m_Score.text = ((int)val).ToString();
            }));
            if(GameManager.Instance.Score > 300)
            {
                m_BombButton.gameObject.SetActive(true);
            }
        }
        private void GameEvents_OnUpdateBombCount()
        {
            m_BombCount.text = DataHandler.Instance.BombCount.ToString();
        }
        private void OnClickBombButton()
        {
            if(DataHandler.Instance.BombCount > 0)
            {
                GameManager.Instance.m_BombSelected = true;
                GameEvents.OnSelectBooster?.Invoke(Booster.Bomb);
            }
            else
            {
                GameEvents.OnGetBooster?.Invoke(Booster.Bomb);
            }
            SoundEvents.OnPlaySingleShotSound?.Invoke("Click");
        }
        public IEnumerator UpdateScore(float start, float end, float seconds, Action<float> onupdate)
        {
            float elapsedTime = 0;
            float startingPos = start;
            while (elapsedTime < seconds)
            {
                var val = Mathf.Lerp(startingPos, end, (elapsedTime / seconds));
                onupdate?.Invoke((int)val);
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            onupdate?.Invoke(end);
        }
        private void OnClickSettingButton()
        {
            InGameUI.Instance.ShowSettingPanel();
        }
    }
}