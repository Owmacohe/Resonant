using System.Linq;
using UnityEngine;

namespace Resonant.Runtime
{
    public class ResonantManager : MonoBehaviour
    {
        [SerializeField] ResonantBehaviour behaviour;
        
        public void Trigger(string id)
        {
            var randomizers =
                FindObjectsByType<ResonantRandomizer>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

            foreach (var i in behaviour.Triggers.Find(trigger => trigger.ID == id).Reactions)
                i.OnReact(this, randomizers.ToList().Where(randomizer => randomizer.ID == i.ID).ToList());
        }
    }
}