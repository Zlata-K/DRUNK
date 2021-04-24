using System.Collections.Generic;
using UnityEngine;

namespace NPCs.Flocking
{
    public abstract class FlockBehaviour : ScriptableObject
    {
        public abstract Vector3 CalculateMove(NPCManager npc, List<Transform> context, FlockManager flock);
    }
}
