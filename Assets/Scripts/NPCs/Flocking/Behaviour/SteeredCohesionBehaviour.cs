using System.Collections.Generic;
using UnityEngine;

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
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(npc, context);
        filteredContext = (filter == null) ? filteredContext : RemoveElementFromContextByName(filteredContext, "RightHand");
        filteredContext = (filter == null) ? filteredContext : RemoveItSelfFromContext(filteredContext, npc);
        
        if(filteredContext.Count == 0)
            return Vector3.zero;
        
        foreach (Transform item in filteredContext)
        {
            cohesionMove += item.position;
        }

        cohesionMove /= filteredContext.Count;

        cohesionMove -= npc.transform.position;
        cohesionMove = Vector3.SmoothDamp(npc.transform.forward, cohesionMove, ref _currentVelocity, agentSmoothTime);
        return cohesionMove;
    }
}