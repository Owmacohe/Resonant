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

        public abstract IEnumerator OnReact(ResonantSource source);
        public void OnReact(MonoBehaviour script, List<ResonantSource> sources) =>
            sources.ForEach(source => script.StartCoroutine(OnReact(source)));
    }
}