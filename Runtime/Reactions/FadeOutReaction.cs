using System;
using System.Collections;
using UnityEngine;

namespace Resonant.Runtime
{
    /// <summary>
    /// If the source is a ResonantRandomizer: Fades the volume out over some amount of time, and stops the loop
    /// If the source is a regular ResonantSource: It fades the AudioClip volume out over some amount of time, then stops it
    /// </summary>
    [Serializable]
    public class FadeOutReaction : ResonantReaction
    {
        /// <summary>
        /// The amount of time that the fade-out should take
        /// </summary>
        public float Time;
        
        public override IEnumerator OnReact(ResonantSource source)
        {
            var randomizer = source.GetComponent<ResonantRandomizer>();
            float startTime = UnityEngine.Time.time;
            float sourceStartVolume = source.Source.volume;
            
            if (randomizer) randomizer.VolumeScale = 1;
            else source.Source.volume = 1;

            while (UnityEngine.Time.time - startTime < Time)
            {
                float volumeScale = 1 - ((UnityEngine.Time.time - startTime) / Time);

                if (randomizer) randomizer.VolumeScale = volumeScale;
                else source.Source.volume = sourceStartVolume * volumeScale;
                
                yield return new WaitForSeconds(STEP_SIZE);
            }

            if (randomizer)
            {
                randomizer.VolumeScale = 0;
                randomizer.StopLoop();   
            }
            else
            {
                source.Source.volume = 0;
                source.Source.Stop();
            }
        }
    }
}