using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Composite")]
public class CompositeBehaviour : FlockBehaviour
{
    [SerializeField] private FlockBehaviour[] behaviours;
    [SerializeField] private float[] weights;

    public FlockBehaviour[] Behaviours
    {
        get => behaviours;
        set => behaviours = value;
    }

    public float[] Weights
    {
        get => weights;
        set => weights = value;
    }
    
    public override Vector3 CalculateMove(NPCManager npc, List<Transform> context, FlockManager flock)
    {
        //handle data mismatch
        if (weights.Length != behaviours.Length)
        {
            Debug.LogError("Data mismatch in " + name, this);
            return Vector3.zero;
        }
        
        Vector3 move = Vector3.zero;

        for(int index = 0; index < behaviours.Length; index++)
        {
            Vector3 partialMove = behaviours[index].CalculateMove(npc, context, flock) * weights[index];

            if (partialMove != Vector3.zero)
            {
                if (partialMove.sqrMagnitude > weights[index] * weights[index])
                {
                    partialMove = partialMove.normalized * weights[index];
                }
                
                move += partialMove;
            }
        }
        move.y = 0;
        return move;
    }
}
