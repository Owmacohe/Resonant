using System;
using System.Collections;

namespace Resonant.Runtime
{
    /// <summary>
    /// If the source is a ResonantRandomizer: Plays a single randomized AudioClip
    /// If the source is a regular ResonantSource: It plays the AudioClip once
    /// </summary>
    [Serializable]
    public class PlayOnceReaction : ResonantReaction
    {
        public override IEnumerator OnReact(ResonantSource source)
        {
            var randomizer = source.GetComponent<ResonantRandomizer>();
            
            if (randomizer) randomizer.PlayOnce();
            else source.Source.PlayOneShot(source.Source.clip);
            
            yield return null;
        }
    }
}