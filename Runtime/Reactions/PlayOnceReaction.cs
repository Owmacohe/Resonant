using System;
using System.Collections;

namespace Resonant.Runtime
{
    [Serializable]
    public class PlayOnceReaction : ResonantReaction
    {
        public override IEnumerator OnReact(ResonantRandomizer randomizer)
        {
            randomizer.PlayOnce();
            yield return null;
        }
    }
}