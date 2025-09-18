using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechJuego.PlanetMerge.Rateus;
namespace TechJuego.PlanetMerge.Rateus
{
    public class RateusHandler : Singleton<RateusHandler>
    {
        protected RateusHandler() { }

        private RateUsData m_RateUsData;
        public void Load()
        {
            m_RateUsData = ResourcesRef.GetRateUsData();
        }

        public void ShowRateus(GameState gameState)
        {

        }
    }
}