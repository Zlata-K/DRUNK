using System.Collections.Generic;
using UnityEngine;

namespace NPCs.Flocking
{
    public abstract class ContextFilter : ScriptableObject
    {
        public abstract List<Transform> Filter(NPCManager npc, List<Transform> original);
    }
}