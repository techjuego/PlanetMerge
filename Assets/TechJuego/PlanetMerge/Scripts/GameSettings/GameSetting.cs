using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TechJuego.PlanetMerge
{
    public class GameSetting
    {
        public static bool GetSound()
        {
            return PlayerPrefs.GetInt("SOUND", 1) > 0;
        }
        public static void SetSound(bool value)
        {
            PlayerPrefs.SetInt("SOUND", value ? 1 : 0);
        }
    }
}