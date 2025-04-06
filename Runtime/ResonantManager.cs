using System.Linq;
using UnityEngine;

namespace Resonant.Runtime
{
    public class ResonantManager : MonoBehaviour
    {
        [SerializeField] ResonantBehaviour behaviour;
        
        public void Trigger(string id)
        {
            var sources = FindObjectsByType<ResonantSource>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

            foreach (var i in behaviour.Triggers.Find(trigger => trigger.ID == id).Reactions)
                i.OnReact(this, sources.ToList().Where(source => source.ID == i.ID).ToList());
        }
    }
}