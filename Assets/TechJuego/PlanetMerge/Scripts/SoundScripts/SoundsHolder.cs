using System.Collections.Generic;
using UnityEngine;

namespace TechJuego.PlanetMerge.Sound
{
    // This scriptable object acts as a container for storing sound clips.
    public class SoundsHolder : ScriptableObject
    {
        // List to store general sound effect clips
        public List<SoundClips> soundClips = new List<SoundClips>();

        // List to store background music clips
        public List<SoundClips> musicClip = new List<SoundClips>();
    }
}
