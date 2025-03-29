using System;
using System.Collections;
using UnityEngine;

namespace Resonant.Runtime
{
    [Serializable]
    public class FadeOutReaction : ResonantReaction
    {
        public float Time;

        readonly float STEP_SIZE = 0.01f;
        
        public override IEnumerator OnReact(ResonantRandomizer randomizer)
        {
            float startTime = UnityEngine.Time.time;

            while (UnityEngine.Time.time - startTime < Time)
            {
                randomizer.volumeScale = 1 - ((UnityEngine.Time.time - startTime) / Time);
                yield return new WaitForSeconds(STEP_SIZE);
            }

            randomizer.volumeScale = 0;
            randomizer.StopLoop();
        }
    }
}