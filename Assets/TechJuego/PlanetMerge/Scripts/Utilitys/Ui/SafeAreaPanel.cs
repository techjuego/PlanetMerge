using UnityEngine;

namespace TechJuego.PlanetMerge.SafeArea
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeAreaPanel : MonoBehaviour
    {
        private RectTransform rectTransform;
        private void OnEnable()
        {
            rectTransform = GetComponent<RectTransform>();
            SetProtrait();
        }
        private string screenOrientation;
        private void OnValidate()
        {
            if (rectTransform != null)
            {
                if (Screen.width > Screen.height)
                {
                    if (screenOrientation != Screen.orientation.ToString())
                    {
                        screenOrientation = Screen.orientation.ToString();
                        SeLandscape();
                    }
                }
                else
                {
                    if (screenOrientation != Screen.orientation.ToString())
                    {
                        screenOrientation = Screen.orientation.ToString();
                        SetProtrait();
                    }
                }
            }
        }
        private void Update()
        {
            if (rectTransform != null)
            {
                if (Screen.width > Screen.height)
                {
                    if (screenOrientation != Screen.orientation.ToString())
                    {
                        screenOrientation = Screen.orientation.ToString();
                        SeLandscape();
                    }
                }
                else
                {
                    if (screenOrientation != Screen.orientation.ToString())
                    {
                        screenOrientation = Screen.orientation.ToString();
                        SetProtrait();
                    }
                }
            }
        }
        void SetProtrait()
        {
            if (screenOrientation == "PortraitUpsideDown")
            {
                rectTransform.sizeDelta = new Vector2(0, -Screen.safeArea.yMin + (Screen.safeArea.height - Screen.safeArea.yMax));
                rectTransform.anchoredPosition = new Vector2(0, 0);
                rectTransform.pivot = new Vector2(0.5f, 0.5f);
            }
            if (screenOrientation == "Portrait")
            {
                rectTransform.sizeDelta = new Vector2(0, -Screen.safeArea.yMin + (Screen.safeArea.height - Screen.safeArea.yMax));
                rectTransform.anchoredPosition = new Vector2(0, 0);
                rectTransform.pivot = new Vector2(0.5f, 0.5f);
            }
        }
        void SeLandscape()
        {
            float heightDifference = 0;
            if (screenOrientation == "LandscapeLeft")
            {
                heightDifference = (Screen.safeArea.height - Screen.safeArea.yMax);
                rectTransform.sizeDelta = new Vector2(-Screen.safeArea.xMin + (Screen.safeArea.width - Screen.safeArea.xMax), heightDifference);
                rectTransform.pivot = new Vector2(0.5f, heightDifference != 0 ? 1 : 0.5f);
                rectTransform.anchoredPosition = new Vector2(0, 0);
            }
            if (screenOrientation == "LandscapeRight")
            {
                heightDifference = (Screen.safeArea.height - Screen.safeArea.yMax);
                rectTransform.sizeDelta = new Vector2(-Screen.safeArea.xMin + (Screen.safeArea.width - Screen.safeArea.xMax), heightDifference);
                rectTransform.pivot = new Vector2(0.5f, heightDifference != 0 ? 1 : 0.5f);
                rectTransform.anchoredPosition = new Vector2(0, 0);
            }
        }
    }
}