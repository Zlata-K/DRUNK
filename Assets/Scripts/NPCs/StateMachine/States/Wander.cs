﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Wander : State
{
    private Queue<Node> _pathToTarget;
    private Vector3 _currentTargetNode;
    private bool reachedTarget = true;
    
    private readonly FlockBehaviour _avoidObstacles;
    private readonly FlockBehaviour _avoidNPCs;

    /*
     * After a random number of moves (between 1 and 10), the NPC stop moving for few seconds.
     */
    private int numberOfMoves = 0;
    private float waitTimeStamp = 0;

    public Wander(NPCManager npcManager, FlockBehaviour avoidObstacles, FlockBehaviour avoidNPCs)
    {
        _npcManager = npcManager;
        numberOfMoves = (int) Random.Range(1.0f, 10.0f);
        _avoidObstacles = avoidObstacles;
        _avoidNPCs = avoidNPCs;
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

        try
        {
            _pathToTarget = new Queue<Node>(Pathfinding.Astar(currentNode, targetNode));
        }
        catch (Exception e)
        {
            Debug.Log(_npcManager.name + " tried to go on a seperated graph. Will stay Idle.");
            _npcManager.GetComponent<NPCStateMachine>().StartIdle();
        }

        if (_pathToTarget == null)
        {
            _npcManager.GetComponent<NPCStateMachine>().StartIdle();
            return;
        }
        
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
        
        Vector3 direction = _currentTargetNode - _npcManager.transform.position;
        direction += _avoidObstacles.CalculateMove(_npcManager, _npcManager.GetNearbyObjects(0.5f), null) * 0.2f;
        direction += _avoidNPCs.CalculateMove(_npcManager, _npcManager.GetNearbyObjects(0.5f), null) * 4f;
        direction = Vector3.Normalize(direction);
        direction.y = 0;
        
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