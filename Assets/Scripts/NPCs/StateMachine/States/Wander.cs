using System.Collections.Generic;
using UnityEngine;

public class Wander : State
{
    private Queue<Node> _pathToTarget;
    private Vector3 _currentTargetNode;
    private bool reachedTarget = true;

    /*
     * After a random number of moves (between 1 and 10), the NPC stop moving for few seconds.
     */
    private int numberOfMoves = 0;
    private float waitTimeStamp = 0;

    public Wander(NPCManager npcManager)
    {
        _npcManager = npcManager;
        numberOfMoves = (int) Random.Range(1.0f, 10.0f);
    }

    public override void Move()
    {
        if (waitTimeStamp < Time.time)
        {
            Wandering();
        }
    }

    private void Wandering()
    {
        if (reachedTarget)
        {
            FindNewTarget();
        }

        MoveTowardsTarget();

        if (TargetReached())
        {
            reachedTarget = true;
            numberOfMoves -= 1;
        }

        if (numberOfMoves == 0)
        {
            StopMoving();
        }
    }

    private void FindNewTarget()
    {
        Vector3 npcPosition = _npcManager.transform.position;
        Vector3 npcVelocity = _npcManager.Rigidbody.velocity;

        Vector3 circlePosition = npcPosition +
                                 Vector3.Normalize(npcVelocity) * NPCsGlobalVariables.WanderCircleDistance;

        Vector3 target = circlePosition + Random.insideUnitSphere * NPCsGlobalVariables.WanderCircleRadius;
        target.y = 0;

        Node currentNode = NavigationGraph.GetClosestNode(npcPosition);
        Node targetNode = NavigationGraph.GetClosestNode(target);

        _pathToTarget = new Queue<Node>(Pathfinding.Astar(currentNode, targetNode));
        _currentTargetNode = _pathToTarget.Dequeue().position;
        _currentTargetNode.y = 0;

        reachedTarget = false;
    }

    private void MoveTowardsTarget()
    {
        if (Vector3.Distance(_npcManager.transform.position, _currentTargetNode) <
            NPCsGlobalVariables.WithinTargetNodeRange && _pathToTarget.Count > 0)
        {
            _currentTargetNode = _pathToTarget.Dequeue().position;
            _currentTargetNode.y = 0;
        }

        Vector3 direction = Vector3.Normalize(_currentTargetNode - _npcManager.transform.position);

        //The NPC will always looking where it needs to go.
        _npcManager.LookWhereYouAreGoing(direction);

        //The NPC always walk forward.
        _npcManager.SetAnimatorVelocity(Vector3.forward *
                                        _npcManager.GetModelSpeed(NPCsGlobalVariables.WanderMaxVelocity));
    }

    private bool TargetReached()
    {
        if (_pathToTarget.Count < 1)
        {
            return Vector3.Distance(_npcManager.transform.position, _currentTargetNode) <
                   NPCsGlobalVariables.WithinTargetNodeRange;
        }

        return false;
    }

    private void StopMoving()
    {
        waitTimeStamp = NPCsGlobalVariables.WanderWaitTime + Time.time;
        numberOfMoves = (int) Random.Range(0.0f, 10.0f);
        _npcManager.SetAnimatorVelocity(Vector3.zero);
    }
}