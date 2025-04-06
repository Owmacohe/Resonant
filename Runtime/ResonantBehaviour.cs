using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Resonant.Runtime
{
    /// <summary>
    /// A ScriptableObject to hold the triggers and reactions for the ResonantBehaviour
    /// </summary>
    [CreateAssetMenu]
    public class ResonantBehaviour : ScriptableObject
    {
        [Tooltip("The list of triggers in the behaviour")] public List<ResonantTrigger> Triggers = new();
    }

    /// <summary>
    /// A trigger within a ResonantBehaviour that performs some actions when it is called
    /// </summary>
    [Serializable]
    public class ResonantTrigger
    {
        [Tooltip("The ID for this trigger (i.e. the name used to call it)")] public string ID;
        [SerializeReference, Tooltip("The runtime reactions that this trigger will perform")] public List<ResonantReaction> Reactions = new();
    }
    
    /// <summary>
    /// A reaction that is performed when a ResonantTrigger is called
    /// </summary>
    /// <remarks>All inheriting classes must use the [Serialized] attribute!</remarks>
    [Serializable]
    public abstract class ResonantReaction
    {
        [Tooltip("The ID of the ResonantSources that this trigger affects")] public string ID;
        
        /// <summary>
        /// The delay (in seconds) between OnReact loops
        /// </summary>
        protected readonly float STEP_SIZE = 0.01f;

        /// <summary>
        /// The actual method that this reaction performs for a single ResonantSource
        /// </summary>
        /// <param name="source">The source that this reaction is applying to</param>
        public abstract IEnumerator OnReact(ResonantSource source);
        
        /// <summary>
        /// A method called to perform this reaction for some amount of sources
        /// </summary>
        /// <param name="manager">The ResonantManager that this response is being called from</param>
        /// <param name="sources">The ResonantSources to apply this reaction to</param>
        public void OnReact(ResonantManager manager, List<ResonantSource> sources) =>
            sources.ForEach(source => manager.StartCoroutine(OnReact(source)));
    }
}