using System;
using UnityEngine;

namespace Resonant.Runtime
{
    [RequireComponent(typeof(AudioSource))]
    public class ResonantSource : MonoBehaviour
    {
        public string ID;
        
        public AudioSource Source { get; protected set; }

        protected void Start()
        {
            Source = GetComponent<AudioSource>();
        }
    }
}