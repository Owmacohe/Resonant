using System;
using System.Collections;
using UnityEngine;

namespace Resonant.Runtime
{
    /// <summary>
    /// Waits for some number of seconds
    /// </summary>
    [Serializable, Wait, Sourceless]
    public class WaitReaction : ResonantReaction
    {
        /// <summary>
        /// The amount of seconds to wait for
        /// </summary>
        public float Seconds;
        
        public override IEnumerator OnReact(ResonantSource source)
        {
            yield return new WaitForSeconds(Seconds);
        }
    }
}