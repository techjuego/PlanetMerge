using TechJuego.FruitSliceMerge.Monetization;
using TechJuego.FruitSliceMerge.Sound;
using UnityEngine;

namespace TechJuego.FruitSliceMerge
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