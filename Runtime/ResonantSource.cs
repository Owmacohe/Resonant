using System;
using UnityEngine;

namespace Resonant.Runtime
{
    /// <summary>
    /// A script used to indicate that the associated AudioSource can be affected by ResonantBehaviours
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class ResonantSource : MonoBehaviour
    {
        [Tooltip("The ID of this source, which is used by ResonantReactions to know which sources they affect")]
        public string ID;
        
        /// <summary>
        /// The AudioSource that this ResonantSource is associated with
        /// </summary>
        public AudioSource Source { get; protected set; }
        
        protected void Start()
        {
            Source = GetComponent<AudioSource>();
        }
    }
}