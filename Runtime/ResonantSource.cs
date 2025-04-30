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
        
        [HideInInspector] public float DefaultVolume; // The default volume that this source started at
        [HideInInspector] public float DefaultPitch; // The default pitch that this source started at

        /// <summary>
        /// Called when this Component is reset via the inspector or is added
        /// </summary>
        void Reset()
        {
            Source = GetComponent<AudioSource>();
            Source.playOnAwake = false;
            Source.volume = 0.5f;
            Source.pitch = 1;
        }
        
        protected void Start()
        {
            Source = GetComponent<AudioSource>();
            DefaultVolume = Source.volume;
            DefaultPitch = Source.pitch;
        }
    }
}