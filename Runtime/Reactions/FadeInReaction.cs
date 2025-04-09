using System;
using System.Collections;
using UnityEngine;

namespace Resonant.Runtime
{
    /// <summary>
    /// If the source is a ResonantRandomizer: Starts the loop, and fades the volume in over some amount of time
    /// If the source is a regular ResonantSource: It plays the AudioClip and fades the volume in over some amount of time
    /// </summary>
    [Serializable, Skippable]
    public class FadeInReaction : ResonantReaction
    {
        /// <summary>
        /// The amount of time that the fade-in should take
        /// </summary>
        public float Time;

        public override IEnumerator OnReact(ResonantSource source)
        {
            var randomizer = source.GetComponent<ResonantRandomizer>();
            float startTime = UnityEngine.Time.time;

            if (randomizer)
            {
                randomizer.VolumeScale = 0;
                randomizer.PlayLoop();
            }
            else
            {
                source.Source.volume = 0;
                source.Source.Play();
            }

            while (UnityEngine.Time.time - startTime < Time)
            {
                float volumeScale = (UnityEngine.Time.time - startTime) / Time;

                if (randomizer) randomizer.VolumeScale = volumeScale;
                else source.Source.volume = volumeScale * source.DefaultVolume;

                yield return new WaitForSeconds(STEP_SIZE);
            }

            if (randomizer) randomizer.VolumeScale = 1;
            else source.Source.volume = source.DefaultVolume;
        }
    }
}