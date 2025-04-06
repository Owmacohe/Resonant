using System;
using System.Collections;
using UnityEngine;

namespace Resonant.Runtime
{
    [Serializable]
    public class LogReaction : ResonantReaction
    {
        public string Message;
        
        public override IEnumerator OnReact(ResonantSource source)
        {
            Debug.Log(Message);
            yield return null;
        }
    }
}