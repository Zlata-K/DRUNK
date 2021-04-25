using Structs;
using UnityEngine;

namespace NPCs.Decision_Tree
{
    public static class NpcDecisionTree
    {
        public static void NpcDetermineAction(Vector3 npcPosition, NpcData npcData)
        {
            if (npcData.LookingForPlayer)
            {
                IsWithinFiveMeters(npcPosition, npcData);
            }
            else
            {
                npcData.StateMachine.StartWandering();
            }
        }

        private static void IsWithinFiveMeters(Vector3 npcPosition, NpcData npcData)
        {
            if (Vector3.Distance(npcPosition, Indestructibles.PlayerData.LastSeenPosition) < 5.0f)
            {
                DetermineChaseType(npcPosition, npcData);
            }
            else
            {
                IsWithinThirtyMeters(npcPosition, npcData);
            }
        }
        
        private static void IsWithinThirtyMeters(Vector3 npcPosition, NpcData npcData)
        {
            if (Vector3.Distance(npcPosition, Indestructibles.PlayerData.LastSeenPosition) < 30.0f)
            {
                Flock(npcData);
            }
            else
            {
                IsWithinFortyMeters(npcPosition, npcData);
            }
        }
        
        
        private static void Flock(NpcData npcData)
        {
            if (npcData.Stuck)
            {
                npcData.StateMachine.StartChasing();
                npcData.StateMachine.Chase.TargetAStarPathNode();
            }
            else
            {
                npcData.StateMachine.StartFlocking();
            }
        }
        
        private static void IsWithinFortyMeters(Vector3 npcPosition, NpcData npcData)
        {
            if (Vector3.Distance(npcPosition, Indestructibles.PlayerData.LastSeenPosition) < 40.0f)
            {
                npcData.StateMachine.StartChasing();
                npcData.StateMachine.Chase.TargetAStarPathNode();
            }
            else
            {
                npcData.StateMachine.StartWandering();
            }
        }

        private static bool DoRaycastOnEnvironment(Vector3 npcPosition)
        {
            const int layerMask = 1 << 8; // only check for collisions on the environment layer
            Vector3 direction = Indestructibles.PlayerData.LastSeenPosition - npcPosition;
            Ray ray = new Ray(npcPosition, direction);
            float maxDistance = Vector3.Distance(npcPosition, Indestructibles.PlayerData.LastSeenPosition);

            return Physics.SphereCast(ray, 0.5f, maxDistance, layerMask);
        }

        private static void DetermineChaseType(Vector3 npcPosition,  NpcData npcData)
        {
            npcData.StateMachine.StartChasing();
            if (DoRaycastOnEnvironment(npcPosition))
            {
                npcData.StateMachine.Chase.TargetAStarPathNode();
            }
            else
            {
                npcData.StateMachine.Chase.TargetPlayerLocation();
            }
        }
    }
}