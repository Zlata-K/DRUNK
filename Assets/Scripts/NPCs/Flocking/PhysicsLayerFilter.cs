using System.Collections.Generic;
using UnityEngine;

namespace NPCs.Flocking
{
    [CreateAssetMenu(menuName = "Flock/Filter/Physics Layer")]
    public class PhysicsLayerFilter : ContextFilter
    {
        [SerializeField] private LayerMask mask;

        public override List<Transform> Filter(NPCManager npc, List<Transform> original)
        {
            List<Transform> filtered = new List<Transform>();
            foreach (Transform item in original)
            {
                if (mask == (mask | (1 << item.gameObject.layer)))
                {
                    filtered.Add(item);
                }
            }
            return filtered;
        }
    }
}