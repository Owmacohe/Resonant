using System;
using System.Collections;

namespace Resonant.Runtime
{
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