using UnityEngine;

namespace TechJuego.FruitSliceMerge.Rateus
{
    // Class responsible for managing the "Rate Us" setting
    public class RateUsSetting
    {
        // Checks if the user has already been prompted to rate
        public static bool IsRatingDone()
        {
            // Returns true if the "RATE" key exists in PlayerPrefs
            return PlayerPrefs.HasKey("RATE");
        }

        // Marks that the user has been prompted to rate
        public static void SetRateDone()
        {
            // Sets the "RATE" key to 1 in PlayerPrefs
            PlayerPrefs.SetInt("RATE", 1);
        }
    }
}
