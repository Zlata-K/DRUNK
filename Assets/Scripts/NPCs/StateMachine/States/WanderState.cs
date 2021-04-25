using System;
using System.Collections.Generic;
using NPCs.Flocking;
using NPCs.Flocking.Behaviour;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NPCs.StateMachine.States
{
    public class WanderState : State
    {
        private Queue<Node> _pathToTarget;
        private Vector3 _currentTargetNode;
        private bool _reachedTarget = true;
        
        /*
         * After a random number of moves (between 1 and 10), the NPC stop moving for few seconds.
         */
        private int _numberOfMoves;
        private float _waitTimeStamp;

        public WanderState(NPCManager npcManager, CollisionAvoidance avoidObstacles, FlockBehaviour avoidNpcs)
        {
            NpcManager = npcManager;
            _numberOfMoves = (int) Random.Range(1.0f, 10.0f);
            AvoidObstacles = avoidObstacles;
            AvoidNpcs = avoidNpcs;
        }

        public override void Move()
        {
            if (_waitTimeStamp < Time.time)
            {
                Wandering();
            }
        }

        private void Wandering()
        {
            if (_reachedTarget)
            {
                FindNewTarget();
            }

            MoveTowardsTarget();

            if (TargetReached())
            {
                _reachedTarget = true;
                _numberOfMoves -= 1;
            }

            if (_numberOfMoves == 0)
            {
                StopMoving();
            }
        }

        private void FindNewTarget()
        {
            Vector3 npcPosition = NpcManager.transform.position;
            Vector3 npcVelocity = NpcManager.Rigidbody.velocity;

            Vector3 circlePosition = npcPosition +
                                     Vector3.Normalize(npcVelocity) * NpcGlobalVariables.WanderCircleDistance;

            Vector3 target = circlePosition + Random.insideUnitSphere * NpcGlobalVariables.WanderCircleRadius;
            target.y = 0;

            Node currentNode = NavigationGraph.GetClosestNode(npcPosition);
            Node targetNode = NavigationGraph.GetClosestNode(target);

            try
            {
                _pathToTarget = new Queue<Node>(Pathfinding.Astar(currentNode, targetNode));
            }
            catch (Exception)
            {
                Debug.Log(NpcManager.name + " tried to go on a separated graph. Will stay Idle.");
                NpcManager.GetComponent<NPCStateMachine>().StartIdle();
            }

            if (_pathToTarget == null || _pathToTarget.Count < 1)
            {
                NpcManager.GetComponent<NPCStateMachine>().StartIdle();
                return;
            }

            _currentTargetNode = _pathToTarget.Dequeue().position;
            _currentTargetNode.y = 0;

            _reachedTarget = false;
        }

        private void MoveTowardsTarget()
        {
            if (Vector3.Distance(NpcManager.transform.position, _currentTargetNode) <
                NpcGlobalVariables.WithinTargetNodeRange && _pathToTarget.Count > 0)
            {
                _currentTargetNode = _pathToTarget.Dequeue().position;
                _currentTargetNode.y = 0;
            }

            var velocity = CalculateVelocity();

            //The NPC will always looking where it needs to go.
            NpcManager.LookWhereYouAreGoing(velocity);

            //The NPC always walk forward.
            NpcManager.SetAnimatorVelocity(Vector3.forward *
                                           NpcManager.GetModelSpeed(NpcGlobalVariables.WanderMaxVelocity));
        }

        private bool TargetReached()
        {
            if (_pathToTarget == null)
            {
                return true;
            }
            
            if (_pathToTarget.Count < 1)
            {
                return Vector3.Distance(NpcManager.transform.position, _currentTargetNode) <
                       NpcGlobalVariables.WithinTargetNodeRange;
            }

            return false;
        }

        private void StopMoving()
        {
            _waitTimeStamp = NpcGlobalVariables.WanderWaitTime + Time.time;
            _numberOfMoves = (int) Random.Range(0.0f, 10.0f);
            NpcManager.SetAnimatorVelocity(Vector3.zero);
        }

        private Vector3 CalculateVelocity()
        {
            Vector3 velocity = _currentTargetNode - NpcManager.transform.position;
            velocity = Vector3.Normalize(velocity);
            velocity += AvoidObstacles.CalculateMove(NpcManager, velocity) *
                        NpcGlobalVariables.WanderObstacleAvoidanceMultiplier;
            velocity.y = 0;

            return velocity;
        }
        
    }
}