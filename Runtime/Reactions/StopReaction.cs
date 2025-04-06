using System;
using System.Collections;

namespace Resonant.Runtime
{
    /// <summary>
    /// If the source is a ResonantRandomizer: Stops the loop and the source from playing
    /// If the source is a regular ResonantSource: Stops the source from playing
    /// </summary>
    [Serializable]
    public class StopReaction : ResonantReaction
    {
        public override IEnumerator OnReact(ResonantSource source)
        {
            var randomizer = source.GetComponent<ResonantRandomizer>();

            if (randomizer)
            {
                randomizer.StopLoop();
                randomizer.Source.Stop();
            }
            else source.Source.Stop();
            
            yield return null;
        }
    }
}