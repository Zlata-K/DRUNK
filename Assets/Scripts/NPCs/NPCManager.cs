using System;
using UnityEditor.Animations;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPCManager : MonoBehaviour
{
    private Rigidbody _playerRigidbody;

    private Rigidbody _rigidbody;
    private Animator _animator;
    private AudioSource _audioSource;
    private NPCStateMachine _stateMachine;
    
    private int _punchLayerIndex;

    private bool _canChase = true;
    private bool _canPunch;
    
    //For some reason, some models are faster than others
    [SerializeField] private float ModelSpeedMultiplier;
    [SerializeField] private AudioClip[] bumpingSounds;
    
    private bool _lookingForPlayer = false;
    
    private bool _punching;

    public Rigidbody PlayerRigidbody => _playerRigidbody;
    public Rigidbody Rigidbody => _rigidbody;
    public Animator Animator => _animator;
    public AudioSource AudioSource => _audioSource;
    public int PunchLayerIndex => _punchLayerIndex;
    public bool LookingForPlayer
    {
        get => _lookingForPlayer;
        set => _lookingForPlayer = value;
    }

    public bool Punching
    {
        get => _punching;
        set => _punching = value;
    }

    void Awake()
    {
        _playerRigidbody = Indestructibles.Player.GetComponent<Rigidbody>();
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _stateMachine = GetComponent<NPCStateMachine>();
        _punchLayerIndex = _animator.GetLayerIndex("Punch Layer");
    }

    private void Update()
    {
        var stateInfo = Animator.GetCurrentAnimatorStateInfo(PunchLayerIndex);
        
        // To let the punching animation finish 
        if (!Punching && stateInfo.normalizedTime % 1 < 0.1f)
        {
            Animator.SetLayerWeight(PunchLayerIndex,0.0f);
        }

        if (Indestructibles.PlayerData.IsKnockedOut)
        {
            _stateMachine.StartWandering();
        }
    }

    public void SetAnimatorVelocity(Vector3 velocity)
    {
        Animator.SetFloat(NPCsGlobalVariables.VelocityXHash, velocity.x);
        Animator.SetFloat(NPCsGlobalVariables.VelocityZHash, velocity.z);
    }
    
    public float LookWhereYouAreGoing(Vector3 direction)
    {
        Quaternion goalRotation = Quaternion.LookRotation(direction);
        Quaternion currentRotation = transform.rotation;
        transform.rotation = Quaternion.RotateTowards(currentRotation, goalRotation, NPCsGlobalVariables.MaxAngleChange * Time.deltaTime);
        return Quaternion.Angle(currentRotation, goalRotation);
    }

    public float GetModelSpeed(float normalSpeed)
    {
        return normalSpeed * ModelSpeedMultiplier;
    }

    public float GetDistanceWithPlayer()
    {
        return Vector3.Distance(Indestructibles.Player.transform.position, transform.position);
    }

    public bool IsChasing()
    {
        return _stateMachine != null && _stateMachine.CurrentState.GetType() == typeof(Chase);
    }
    public void StartPunching()
    {
        if (_canPunch && !Punching)
        {
            Punching = true;
            Animator.Play("Punch", PunchLayerIndex, 0f);
            Animator.SetLayerWeight(PunchLayerIndex,1.0f); 
        }
    }

    public void StopPunching()
    {
        Punching = false;
    }

    public void OnPlayerHit()
    {
        _stateMachine.StartWandering();
        StopPunching();

        _canChase = false;
        Invoke(nameof(ChaseCooldown), 3.0f);
    }

    private void ChaseCooldown()
    {
        _canChase = true;
    }
    private void PunchCooldown()
    {
        _canPunch = true;
    }
    /*
     * If the player collides with a NPC that is not chasing him, start chase.
     * For now, the collider is of type trigger because there is no navmesh.
     */
    void OnCollisionEnter(Collision collision)
    {
        if (_canChase && !IsChasing() && collision.gameObject.CompareTag("Player"))
        {
            
            if (bumpingSounds.Length > 0)
            {
                AudioSource.PlayOneShot(bumpingSounds[Random.Range(0,bumpingSounds.Length-1)]);
            }
            
            Animator.SetTrigger(Animator.StringToHash("Get Hit"));
            StartChasing();
        }
    }

    public void StartChasing()
    {
        _stateMachine.StartChasing();

        _canPunch = false;
        Invoke(nameof(PunchCooldown), 1.0f);
    }
}
