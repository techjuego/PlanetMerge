using System;
using UnityEngine;
namespace TechJuego.FruitSliceMerge
{
    public class DropLine : MonoBehaviour
    {
        public Transform m_Line;
        private void OnEnable()
        {
            GameEvents.OnMosueDown += GameEvents_OnMosueDown;
            GameEvents.OnMosueUp += GameEvents_OnMosueUp;
        }
        private void OnDisable()
        {
            GameEvents.OnMosueDown -= GameEvents_OnMosueDown;
            GameEvents.OnMosueUp -= GameEvents_OnMosueUp;
        }
        private void GameEvents_OnMosueUp()
        {
            m_Line.gameObject.SetActive(false);
        }
        private void GameEvents_OnMosueDown(Vector3 pos)
        {
            m_Line.gameObject.SetActive(true);
            m_Line.transform.position = new Vector3(pos.x,0, pos.y);
        }
    }
}