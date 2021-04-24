using System.Collections.Generic;
using Structs;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlockManager : MonoBehaviour
{
    [SerializeField] private FlockBehaviour behavior;

    private List<NPCManager> _agents = new List<NPCManager>();

    //Flocking behaviours variables
    [SerializeField] [Range(1f, 100f)] private float driveFactor = 10f;
    [SerializeField] [Range(1f, 100f)] private float maxSpeed = 5f;
    [SerializeField] [Range(1f, 10f)] private float neighborRadius = 5f;
    [SerializeField] [Range(0f, 1f)] private float avoidanceRadiusMultiplier = 0.5f;

    private float squareMaxSpeed;
    private float squareNeighborRadius;
    private float squareAvoidanceRadius;

    //Chase variables
    private Vector3 _currentTargetLocation = NPCsGlobalVariables.DefaultInitialVector;
    private PlayerData _playerData;
    private bool agentReachedNode = true;
    private NPCManager closestAgentToPlayer;
    private List<Node> pathTest = new List<Node>();

    public float SquareAvoidanceRadius => squareAvoidanceRadius;

    public Vector3 CurrentTargetLocation => _currentTargetLocation;

    void Start()
    {
        _playerData = Indestructibles.PlayerData;
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;
    }
    
    private void OnDrawGizmos()
    {

        foreach (var node in pathTest) 
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(node.position, 0.5F);

            /*foreach (var link in node.links) {
                Gizmos.color = Color.black;
                Gizmos.DrawLine(node.position, link.node.position);
            }*/
        }
    }

    private void Update()
    {
        if (_agents.Count < 1)
            return;

        if (agentReachedNode)
        {
            ComputeAStar();
            agentReachedNode = false;
        }

        foreach (NPCManager npc in _agents)
        {
            List<Transform> context = npc.GetNearbyObjects(neighborRadius);
            Vector3 move = behavior.CalculateMove(npc, context, this);
            move *= driveFactor;
            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }

            npc.Move(move);
        }
    }

    private void ComputeAStar()
    {
        Vector3 nodeWithoutY = Vector3.zero;
        if (_currentTargetLocation != NPCsGlobalVariables.DefaultInitialVector)
        {
            nodeWithoutY = new Vector3(_currentTargetLocation.x, 0, _currentTargetLocation.z);
        }

        Node npcNode = NavigationGraph.GetClosestNode(GetPositionOfClosestAgent());
        Node playerNode = NavigationGraph.GetClosestNode(_playerData.LastSeenPosition);

        List<Node> path = Pathfinding.Astar(npcNode, playerNode);
        pathTest = path;

        if (path == null)
        {
            Debug.Log(npcNode);
        }

        if (path.Count < 2)
        {
            _currentTargetLocation = _playerData.LastSeenPosition;
        }
        else
        {
            _currentTargetLocation = path[NPCsGlobalVariables.NextElementInPath].position;
        }
    }

    private Vector3 GetPositionOfClosestAgent()
    {
        NPCManager closestAgent = null;
        float closestdDistance = -1;
        
        if (_agents.Count == 1)
        {
            closestAgent = _agents[0];
        }
        else
        {
            foreach (NPCManager agent in _agents)
            {
                float agentDistance = agent.GetDistanceWithPlayer();
                if (closestdDistance < 0 || agentDistance < closestdDistance)
                {
                    closestdDistance = agentDistance;
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

    public void AddNPCToFlock(NPCManager npc)
    {
        _agents.Add(npc);
        ComputeAStar();
    }

    public void RemoveNPCFromFlock(NPCManager npc)
    {
        _agents.Remove(npc);
        ComputeAStar();
    }

    public bool IsNPCInFlock(NPCManager npc)
    {
        return _agents.Contains(npc);
    }

    public void AgentReachedTarget(NPCManager npc)
    {
        agentReachedNode = true;
    }
}