using System;
using System.Collections;
using UnityEngine;

namespace Resonant.Runtime
{
    /// <summary>
    /// If the source is a ResonantRandomizer: Plays the randomized loop
    /// If the source is a regular ResonantSource: It plays the AudioClip repeatedly
    /// </summary>
    [Serializable, Skippable]
    public class PlayLoopReaction : ResonantReaction
    {
        /// <summary>
        /// The amount of time that the loop should play for
        /// (set to -1 to loop indefinitely)
        /// </summary>
        public float Time = -1;
        
        public override IEnumerator OnReact(ResonantSource source)
        {
            var randomizer = source.GetComponent<ResonantRandomizer>();
            
            if (randomizer) randomizer.PlayLoop(Time);
            else
            {
                source.Source.loop = true;
                source.Source.Play();
            }
            
            yield return new WaitForSeconds(Time);
            source.Source.Stop();
        }
    }
}