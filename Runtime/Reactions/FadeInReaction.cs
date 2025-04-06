using System;
using System.Collections;
using UnityEngine;

namespace Resonant.Runtime
{
    [Serializable]
    public class FadeInReaction : ResonantReaction
    {
        public float Time;

        readonly float STEP_SIZE = 0.01f;

        public override IEnumerator OnReact(ResonantSource source)
        {
            var randomizer = source.GetComponent<ResonantRandomizer>();
            float startTime = UnityEngine.Time.time;

            if (randomizer)
            {
                randomizer.volumeScale = 0;
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

                if (randomizer) randomizer.volumeScale = volumeScale;
                else source.Source.volume = volumeScale;

                yield return new WaitForSeconds(STEP_SIZE);
            }

            if (randomizer) randomizer.volumeScale = 1;
            else source.Source.volume = 1;
        }
    }
}