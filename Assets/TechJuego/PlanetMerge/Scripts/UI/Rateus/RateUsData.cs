using UnityEngine;

namespace TechJuego.FruitSliceMerge.Rateus
{
    /// <summary>
    /// Properties from the settings window for handling "Rate Us" functionality.
    /// </summary>
    public class RateUsData : ScriptableObject
    {
        // Apple App Store ID for the iOS app
        public string iosAppID;

        // Google Play Store bundle ID for the Android app
        public string googlePlayBundleID;

        // Game state when the "Rate Us" prompt should be shown
        public GameState WhenToShow = GameState.None;

        // Interval at which the "Rate Us" prompt should be displayed (e.g., every 5th occurrence)
        public int CallOnEvery = 5;

        // Current count of times the prompt has been shown or should be shown
        public int CallCount;

        public RateUsData DeepCopy()
        {
            var other = (RateUsData)MemberwiseClone();
            return other;
        }
    }
}
