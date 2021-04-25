using System;
using System.Collections.Generic;
using NPCs.Flocking.Behaviour;
using Player;
using Structs;
using UnityEngine;

namespace NPCs.StateMachine.States
{
    public class ChaseState : State
    {
        private readonly PlayerData _playerData;
        private Vector3 _currentTargetLocation = NpcGlobalVariables.DefaultInitialVector;

        public ChaseState(NPCManager npcManager, CollisionAvoidance avoidObstacles)
        {
            NpcManager = npcManager;
            _playerData = Indestructibles.Player.GetComponent<PlayerDataManager>().PlayerData;
            AvoidObstacles = avoidObstacles;
        }

        public void TargetPlayerLocation()
        {
            _currentTargetLocation = _playerData.LastSeenPosition;
        }
        
        public void TargetAStarPathNode()
        {
            try
            {
                ComputeAStarForSteering();
            }
            catch (Exception)
            {
                _currentTargetLocation = _playerData.LastSeenPosition;
            }
        }
        
        public override void Move()
        {
            Chasing();
        }
        
        private void Chasing()
        {
            Vector3 velocity = ComputeVelocity();

            //The NPC will always looking where it needs to go.
            float anglePlayerNpc = NpcManager.LookWhereYouAreGoing(velocity);

            //If the player is out FOV, NPC stop moving and rotate until the player is back in FOV
            if (anglePlayerNpc > NpcGlobalVariables.FieldOfView &&
                NpcManager.GetDistanceWithPlayer() > NpcGlobalVariables.ChasePlayerRange)
            {
                velocity = Vector3.zero;
            }

            //The NPC always walk forward.
            NpcManager.SetAnimatorVelocity(Vector3.forward * velocity.magnitude);
        }

        private void ComputeAStarForSteering()
        {
            Node npcNode = NavigationGraph.GetClosestNode(NpcManager.transform.position);
            Node playerNode = NavigationGraph.GetClosestNode(_playerData.LastSeenPosition);

            List<Node> path = Pathfinding.Astar(npcNode, playerNode);

            if (path == null || path.Count < 1)
            {
                Debug.Log(npcNode);
                return;
            }

            _currentTargetLocation = path[NpcGlobalVariables.NextElementInPath].position;
        }
        
        private Vector3 ComputeVelocity()
        {
            Vector3 desiredVelocity = Vector3.Normalize(_currentTargetLocation - NpcManager.transform.position) *
                                      NpcManager.GetModelSpeed(NpcGlobalVariables.ChaseAcceleration);

            Vector3 currentVelocity = NpcManager.Rigidbody.velocity;

            Vector3 steering = desiredVelocity - currentVelocity;
            
            if (steering.magnitude > NpcGlobalVariables.ChaseAcceleration)
            {
                steering = (steering / Vector3.Magnitude(steering)) *
                           NpcManager.GetModelSpeed(NpcGlobalVariables.ChaseAcceleration);
            }

            Vector3 velocity = currentVelocity + steering;

            if (Vector3.Magnitude(velocity) > NpcManager.GetModelSpeed(NpcGlobalVariables.ChaseMaxVelocity))
            {
                velocity = (velocity / Vector3.Magnitude(velocity)) *
                           NpcManager.GetModelSpeed(NpcGlobalVariables.ChaseMaxVelocity);
            }
            
            velocity += AvoidObstacles.CalculateMove(NpcManager, velocity) *
                        NpcGlobalVariables.ChaseObstacleAvoidanceMultiplier;

            velocity.y = 0;

            return velocity;
        }
    }
}