using System.Collections.Generic;
using UnityEngine;

namespace NPCs.Flocking.Behaviour
{
    [CreateAssetMenu(menuName = "Flock/Behaviour/SteeredCohesion")]

//Move towards the average position of all the neighbour agents
    public class SteeredCohesionBehaviour : FilteredFlockBehavior
    {
        [SerializeField] private float agentSmoothTime = 0.5f;
        private Vector3 _currentVelocity;

        public override Vector3 CalculateMove(NPCManager npc, List<Transform> context, FlockManager flock)
        {
            _currentVelocity = npc.Rigidbody.velocity;
            Vector3 cohesionMove = Vector3.zero;

            var filteredContext = FilterContext(npc, context);

            if (filteredContext.Count == 0)
                return Vector3.zero;

            foreach (Transform item in filteredContext)
            {
                cohesionMove += item.position;
            }

            cohesionMove /= filteredContext.Count;

            var transform = npc.transform;

            cohesionMove -= transform.position;
            cohesionMove = Vector3.SmoothDamp(transform.forward, cohesionMove, ref _currentVelocity, agentSmoothTime);
            return cohesionMove;
        }

        private List<Transform> FilterContext(NPCManager npc, List<Transform> context)
        {
            List<Transform> filteredContext = (filter == null) ? context : filter.Filter(npc, context);
            filteredContext = filter == null
                ? filteredContext
                : RemoveElementFromContextByName(filteredContext, "RightHand");

            return filter == null ? filteredContext : RemoveItSelfFromContext(filteredContext, npc);
        }
    }
}