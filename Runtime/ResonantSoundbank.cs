using System.Collections.Generic;
using Resonant.Runtime;
using UnityEngine;

namespace Resonant.Runtime
{
    /// <summary>
    /// A list of sounds that can be played using a ResonantReaction
    /// </summary>
    public class ResonantSoundbank : ResonantSource
    {
        [Tooltip("All of the clips that this source can play")] public List<AudioClip> Clips;

        public void PlayOnce(int index)
        {
            Source.clip = Clips[index];
            Source.Play();
        }
    }
}