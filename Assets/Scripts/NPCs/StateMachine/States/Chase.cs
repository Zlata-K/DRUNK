using System;
using System.Collections.Generic;
using NPCs.Flocking;
using NPCs.Flocking.Behaviour;
using Player;
using Structs;
using UnityEngine;

namespace NPCs.StateMachine.States
{
    public class Chase : State
    {
        private readonly PlayerData _playerData;
        private Vector3 _currentTargetLocation = NpcGlobalVariables.DefaultInitialVector;
        private readonly LayerMask _environmentMask;

        public Chase(NPCManager npcManager, CollisionAvoidance avoidObstacles, FlockBehaviour avoidNpcs)
        {
            NpcManager = npcManager;
            _playerData = Indestructibles.Player.GetComponent<PlayerDataManager>().PlayerData;
            _environmentMask = LayerMask.GetMask("Environment");
            AvoidObstacles = avoidObstacles;
            AvoidNpcs = avoidNpcs;
        }

        public override void Move()
        {
            Chasing();
        }

        private void Chasing()
        {
            Vector3 velocity = ComputeChaseVelocity();

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

        private Vector3 ComputeChaseVelocity()
        {
            ComputeCurrentTargetLocation();
            
            return ComputeVelocity();
        }
        
        
        private void ComputeCurrentTargetLocation()
        {
            Vector3 direction = (_playerData.LastSeenPosition - NpcManager.transform.position);
            Ray ray = new Ray(NpcManager.transform.position, direction);
            float maxDistance = Mathf.Min(NpcGlobalVariables.ChasePlayerRange,
                Vector3.Distance(NpcManager.transform.position, _playerData.LastSeenPosition));
            
            if (!Physics.SphereCast(ray, 0.5f, maxDistance, _environmentMask))
            {
                _currentTargetLocation = _playerData.LastSeenPosition;
            }
            else
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
        }

        private void ComputeAStarForSteering()
        {
            Node npcNode = NavigationGraph.GetClosestNode(NpcManager.transform.position);
            Node playerNode = NavigationGraph.GetClosestNode(_playerData.LastSeenPosition);

            List<Node> path = Pathfinding.Astar(npcNode, playerNode);

            if (path == null)
            {
                Debug.Log(npcNode);
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