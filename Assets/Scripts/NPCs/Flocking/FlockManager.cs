using System.Collections.Generic;
using Structs;
using UnityEngine;

namespace NPCs.Flocking
{
    public class FlockManager : MonoBehaviour
    {
        [SerializeField] private FlockBehaviour behavior;

        private List<NPCManager> _agents = new List<NPCManager>();

        //Flocking behaviours variables
        [SerializeField] [Range(1f, 100f)] private float driveFactor = 10f;
        [SerializeField] [Range(1f, 100f)] private float maxSpeed = 5f;
        [SerializeField] [Range(1f, 10f)] private float neighborRadius = 5f;

        private float _squareMaxSpeed;

        //Chase variables
        private Vector3 _currentTargetLocation = NpcGlobalVariables.DefaultInitialVector;
        private PlayerData _playerData;
        private bool _agentReachedNode = true;
        private NPCManager _closestAgentToPlayer;

        public Vector3 CurrentTargetLocation => _currentTargetLocation;

        private void Start()
        {
            _playerData = Indestructibles.PlayerData;
            _squareMaxSpeed = maxSpeed * maxSpeed;
        }

        private void Update()
        {
            if (_agents.Count < 1)
                return;

            if (_agentReachedNode)
            {
                ComputeAStarForFlock();
                _agentReachedNode = false;
            }

            MoveAgents();
        }

        private void ComputeAStarForFlock()
        {
            Node npcNode = NavigationGraph.GetClosestNode(GetPositionOfClosestAgent());
            Node playerNode = NavigationGraph.GetClosestNode(_playerData.LastSeenPosition);

            List<Node> path = Pathfinding.Astar(npcNode, playerNode);

            if (path == null)
            {
                Debug.Log($"The A* path is null. Last seen player position is {_playerData.LastSeenPosition}");
            }

            if (path.Count < 2)
            {
                _currentTargetLocation = _playerData.LastSeenPosition;
            }
            else
            {
                _currentTargetLocation = path[NpcGlobalVariables.NextElementInPath].position;
            }
        }

        private Vector3 GetPositionOfClosestAgent()
        {
            NPCManager closestAgent = null;
            float closestDistance = -1;

            if (_agents.Count == 1)
            {
                closestAgent = _agents[0];
            }
            else
            {
                foreach (NPCManager agent in _agents)
                {
                    float agentDistance = agent.GetDistanceWithPlayer();
                    if (closestDistance < 0 || agentDistance < closestDistance)
                    {
                        closestDistance = agentDistance;
                        closestAgent = agent;
                    }
                }
            }

            if (closestAgent == null)
            {
                return Vector3.zero;
            }

            return closestAgent.transform.position;
        }

        public void AddNpcToFlock(NPCManager npc)
        {
            _agents.Add(npc);
            ComputeAStarForFlock();
        }

        public void RemoveNpcFromFlock(NPCManager npc)
        {
            _agents.Remove(npc);
            ComputeAStarForFlock();
        }

        public bool IsNpcInFlock(NPCManager npc)
        {
            return _agents.Contains(npc);
        }

        public void AgentReachedTarget(NPCManager npc)
        {
            _agentReachedNode = true;
        }

        private void MoveAgents()
        {
            foreach (NPCManager npc in _agents)
            {
                List<Transform> context = npc.GetNearbyObjects(neighborRadius);
                Vector3 velocity = behavior.CalculateMove(npc, context, this);
                velocity *= driveFactor;
                if (velocity.sqrMagnitude > _squareMaxSpeed)
                {
                    velocity = velocity.normalized * maxSpeed;
                }

                npc.LookWhereYouAreGoing(velocity);
                npc.SetAnimatorVelocity(npc.GetModelSpeed(velocity.magnitude) * Vector3.forward);
            }
        }
    }
}