using System.Linq;
using UnityEngine;

namespace Resonant.Runtime
{
    /// <summary>
    /// The runtime manager that applies ResonantReactions to ResonantSources when ResonantTriggers are called
    /// </summary>
    public class ResonantManager : MonoBehaviour
    {
        [SerializeField, Tooltip("The ResonantBehaviour that's triggers and reactions will be applied")] ResonantBehaviour behaviour;
        
        /// <summary>
        /// Triggers the ResonantTriggers for a particular ID
        /// </summary>
        /// <param name="id">The ID of the ResonantTriggers to be triggered</param>
        public void Trigger(string id)
        {
            var sources = FindObjectsByType<ResonantSource>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).ToList();

            // Getting all the ResonantTriggers that match the ID, and then triggering all their ResonantReactions
            foreach (var i in behaviour.Triggers.Where(trigger => trigger.ID == id))
                foreach (var j in i.Reactions)
                    j.OnReact(this, sources.Where(source => source.ID == j.ID).ToList());
        }
    }
}