using UnityEngine;

namespace TechJuego.FruitSliceMerge.HapticFeedback
{
    public class HapticSetting
    {
        public static readonly string HapticViration = "VIBRATE";
        public static bool GetVibrate()
        {
            return PlayerPrefs.GetInt(HapticViration, 1) > 0;
        }
        public static void SetVibrate(bool value)
        {
            PlayerPrefs.SetInt(HapticViration, value ? 1 : 0);
        }
    }
}