using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace TechJuego.PlanetMerge.Utils
{
    public class UiUtility
    {
        public static void SetButton(Button button, Action action)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => { action?.Invoke(); });
        }
        public static void SetToggle(Toggle toggle, UnityAction<bool> action)
        {
            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener(action);
        }
        public static IEnumerator Updatelayout(VerticalLayoutGroup group)
        {
            yield return new WaitForEndOfFrame();
            group.spacing += 0.1f;
            group.spacing -= 0.1f;
        }
        public static IEnumerator Updatelayout(HorizontalLayoutGroup group)
        {
            yield return new WaitForEndOfFrame();
            group.spacing += 0.1f;
            group.spacing -= 0.1f;
        }
        public static bool IsPointerOverUIObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
    }
}