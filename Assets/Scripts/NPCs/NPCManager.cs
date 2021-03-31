using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    private Vector3 _velocity;
    private Vector3 _previousPosition;

    private GameObject _player;
    private bool _lookingForPlayer = false;

    public Vector3 Velocity => _velocity;
    public Vector3 PreviousPosition => _previousPosition;

    public bool LookingForPlayer
    {
        get => _lookingForPlayer;
        set => _lookingForPlayer = value;
    }

    void Start()
    {
        _previousPosition = transform.position;
        _player = GameObject.Find("Player");
    }
    
    /*
     * Since the rigidbody is not directly on the game object that is moving. We cannot use it to get the velocity.
     * The velocity needs to be computed manually.
     */
    void Update()
    {
        _velocity = (transform.position - _previousPosition) / Time.deltaTime;
        _previousPosition = transform.position;
        
        
    }
}
