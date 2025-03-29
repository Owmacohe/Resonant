using System;
using System.Collections.Generic;
using UnityEngine;

namespace Resonant.Runtime
{
    [CreateAssetMenu]
    public class ResonantBehaviour : ScriptableObject
    {
        public List<ResonantTrigger> Triggers = new();
    }

    [Serializable]
    public class ResonantTrigger
    {
        public string ID;
        [SerializeReference] public List<ResonantReaction> Reactions = new();
    }
}