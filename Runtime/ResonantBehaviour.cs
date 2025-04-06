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
    /// <remarks>
    /// All inheriting classes must use the [Serialized] attribute and their names must end in 'Reaction'!
    /// </remarks>
    [Serializable]
    public abstract class ResonantReaction
    {
        [Tooltip("The ID of the ResonantSources that this trigger affects")] public string ID;
        
        /// <summary>
        /// Whether to wait until this reaction has finished before moving on to the next one
        /// </summary>
        [HideInInspector] public bool WaitUntilEnd;
        
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
        public IEnumerator OnReact(ResonantManager manager, List<ResonantSource> sources)
        {
            for (int i = 0; i < sources.Count; i++)
            {
                var reaction = manager.StartCoroutine(OnReact(sources[i]));
                if (i == 0) yield return reaction;
            }
        }
    }

    /// <summary>
    /// The attribute used to tell the editor to expose the WaitUntilEnd toggle
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SkippableAttribute : Attribute { }

    /// <summary>
    /// The attribute used to tell the ResonantManager to yield return the reaction
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class WaitAttribute : Attribute { }

    /// <summary>
    /// The attribute used to indicate that this reaction requires no ResonantSource to function
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SourcelessAttribute : Attribute { }
}