using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Avoidance")]

//Move agent away from the neighbour agents
public class AvoidanceBehaviour : FilteredFlockBehavior
{
    public override Vector3 CalculateMove(NPCManager npc, List<Transform> context, FlockManager flock)
    {
        Vector3 avoidanceMove = Vector3.zero;
        int nAvoid = 0;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(npc, context);
        filteredContext = (filter == null) ? filteredContext : RemoveElementFromContextByName(filteredContext, "Ground");
        filteredContext = (filter == null) ? filteredContext : RemoveElementFromContextByName(filteredContext, "RightHand");
        filteredContext = (filter == null) ? filteredContext : RemoveItSelfFromContext(filteredContext, npc);
        if(filteredContext.Count == 0)
            return Vector3.zero;
        
        foreach (Transform item in filteredContext)
        {
            nAvoid++;
            avoidanceMove += (npc.transform.position - item.position);
            /*if (Vector3.SqrMagnitude(item.position - npc.transform.position) < flock.SquareAvoidanceRadius)
            {
                nAvoid++;
                avoidanceMove += (npc.transform.position - item.position);
            }*/
        }

        if (nAvoid > 0)
        {
            avoidanceMove /= nAvoid;

        }

        return avoidanceMove;
    }
}
