using UnityEngine;

namespace TechJuego.PlanetMerge.Sound
{
    public class SoundSetting
    {
        public static readonly string MusicVariable = "MUSIC";
        public static bool GetMusic()
        {
            return PlayerPrefs.GetInt(MusicVariable, 1) > 0;
        }
        public static void SetSound(bool value)
        {
            PlayerPrefs.SetInt(MusicVariable, value ? 1 : 0);
        }
        public static readonly string SfxVariable = "SFX";
        public static bool GetSFX()
        {
            return PlayerPrefs.GetInt(SfxVariable, 1) > 0;
        }
        public static void SetSFX(bool value)
        {
            PlayerPrefs.SetInt(SfxVariable, value ? 1 : 0);
        }
    }
}
