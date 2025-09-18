using TechJuego.PlanetMerge.Monetization;
using TechJuego.PlanetMerge.Sound;
using UnityEngine;

namespace TechJuego.PlanetMerge
{
    public class AutoInitializer
    {
        [RuntimeInitializeOnLoadMethod]
        static void OnRuntimeMethodLoad()
        {
            AdsHandler.Instance.Load();
            SoundManager.Instance.Load();
        }
    }
}