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
        
        public override IEnumerator OnReact(ResonantRandomizer randomizer)
        {
            float startTime = UnityEngine.Time.time;

            randomizer.PlayLoop();

            while (UnityEngine.Time.time - startTime < Time)
            {
                randomizer.volumeScale = (UnityEngine.Time.time - startTime) / Time;
                yield return new WaitForSeconds(STEP_SIZE);
            }
            
            randomizer.volumeScale = 1;
        }
    }
}