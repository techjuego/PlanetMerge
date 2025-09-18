using UnityEngine;
using System;

namespace TechJuego.PlanetMerge.Sound
{
    // Serializable class for managing sound clips
    [Serializable]
    public class SoundClips
    {
        // Name of the sound clip for identification
        public string clipName;

        // The actual audio clip asset
        public AudioClip clip;

        // Volume level for the sound clip, ranging from 0 (silent) to 1 (full volume)
        [Range(0, 1)]
        public float volume = 1;
    }
}
