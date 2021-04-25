using Structs;
using UnityEngine;

namespace NPCs.Decision_Tree
{
    public static class NpcDecisionTree
    {
        public static void NpcDetermineAction(Vector3 npcPosition, NPCManager npcManager)
        {
            if (npcManager._npcData.LookingForPlayer)
            {
                IsWithinFiveMeters(npcPosition, npcManager);
            }
            else
            {
                npcManager._npcData.StateMachine.StartWandering();
            }
        }

        private static void IsWithinFiveMeters(Vector3 npcPosition,  NPCManager npcManager)
        {
            if (Vector3.Distance(npcPosition, Indestructibles.PlayerData.LastSeenPosition) < 5.0f)
            {
                DetermineChaseType(npcPosition, npcManager);
            }
            else
            {
                IsWithinThirtyMeters(npcPosition, npcManager);
            }
        }
        
        private static void IsWithinThirtyMeters(Vector3 npcPosition, NPCManager npcManager)
        {
            if (Vector3.Distance(npcPosition, Indestructibles.PlayerData.LastSeenPosition) < 30.0f)
            {
                Flock(npcManager);
            }
            else
            {
                IsWithinFortyMeters(npcPosition, npcManager);
            }
        }
        
        
        private static void Flock(NPCManager npcManager)
        {
            if (npcManager._npcData.Stuck)
            {
                npcManager.StartChasing();
                npcManager._npcData.StateMachine.Chase.TargetAStarPathNode();
            }
            else
            {
                npcManager._npcData.StateMachine.StartFlocking();
            }
        }
        
        private static void IsWithinFortyMeters(Vector3 npcPosition, NPCManager npcManager)
        {
            if (Vector3.Distance(npcPosition, Indestructibles.PlayerData.LastSeenPosition) < 40.0f)
            {
                npcManager.StartChasing();
                npcManager._npcData.StateMachine.Chase.TargetAStarPathNode();
            }
            else
            {
                npcManager._npcData.StateMachine.StartWandering();
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

        private static void DetermineChaseType(Vector3 npcPosition,   NPCManager npcManager)
        {
            npcManager.StartChasing();
            if (DoRaycastOnEnvironment(npcPosition))
            {
                npcManager._npcData.StateMachine.Chase.TargetAStarPathNode();
            }
            else
            {
                npcManager._npcData.StateMachine.Chase.TargetPlayerLocation();
            }
        }
    }
}