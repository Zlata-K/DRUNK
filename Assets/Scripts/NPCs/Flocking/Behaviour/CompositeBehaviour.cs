using System.Collections.Generic;
using UnityEngine;

namespace NPCs.Flocking.Behaviour
{
    [CreateAssetMenu(menuName = "Flock/Behaviour/Composite")]
    public class CompositeBehaviour : FlockBehaviour
    {
        [SerializeField] private FlockBehaviour[] behaviours;
        [SerializeField] private float[] weights;

        [SerializeField] private CollisionAvoidance _collisionAvoidance;

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
            if (Weights.Length != Behaviours.Length)
            {
                Debug.LogError("Data mismatch in " + name, this);
                return Vector3.zero;
            }
        
            Vector3 move = Vector3.zero;

            for(int index = 0; index < Behaviours.Length; index++)
            {
                Vector3 partialMove = Behaviours[index].CalculateMove(npc, context, flock) * Weights[index];

                if (partialMove != Vector3.zero)
                {
                    if (partialMove.sqrMagnitude > Weights[index] * Weights[index])
                    {
                        partialMove = partialMove.normalized * Weights[index];
                    }
                
                    move += partialMove;
                }
            }

            move.y = 0;
            return move;
        }
    }
}
