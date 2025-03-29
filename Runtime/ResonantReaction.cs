using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Resonant.Runtime
{
    [Serializable]
    public abstract class ResonantReaction
    {
        public string ID;

        public abstract IEnumerator OnReact(ResonantRandomizer randomizer);
        public void OnReact(MonoBehaviour source, List<ResonantRandomizer> randomizers) =>
            randomizers.ForEach(randomizer => source.StartCoroutine(OnReact(randomizer)));
    }
}