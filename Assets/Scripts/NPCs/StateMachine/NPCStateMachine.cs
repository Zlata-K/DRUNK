using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStateMachine: MonoBehaviour
{
    private State _currentState;

    private State _wander;
    private State _idle;
    private State _chase;
    
    private GameObject _player;

    private float _maxDistance = 10f;

    void Start()
    {
        _wander = new Wander();
        _chase = new Chase();
        _idle = new Idle();

        _currentState = _wander;
        _player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        //CheckForStopChasing();
        _currentState.Move(_player, gameObject);
    }

    private void CheckForStopChasing()
    {
        if (Vector3.Distance(_player.transform.position, transform.position) > _maxDistance
                && _currentState == _chase)
        {
            _currentState = _wander;
        }
            
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            _currentState = _chase;
        }
    }
}
