using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Alignment")]

//Align the rotation of the agent according to the neighbour agents
public class AlignmentBehaviour : FilteredFlockBehavior
{
    public override Vector3 CalculateMove(NPCManager npc, List<Transform> context, FlockManager flock)
    {
        Vector3 alignmentMove = Vector3.zero;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(npc, context);
        
        if(filteredContext.Count == 0)
            return Vector3.zero;
        
        foreach (Transform item in context)
        {
            alignmentMove += item.transform.forward;
        }

        alignmentMove /= filteredContext.Count;
        
        return alignmentMove;
    }
}
